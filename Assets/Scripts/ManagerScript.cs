using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;
using System;
using TMPro;

public class ManagerScript : MonoBehaviour
{
    private bool begin = false;
    public GameObject sphere;
    public GameObject dialogConfirmation;
    private GestureRecognizer recognizer;
    private int count;
    private bool recording = false;
    private LineRenderer lineRenderer;
    private List<Vector3> fingerPositions = new List<Vector3>();
    private List<Vector3> relativeCoordinates = new List<Vector3>();
    private Vector3 relativePosition;
    public GameObject gizmo;
    public TextMesh coordinates;
    public GesturesPublisher gesturesPublisher;
    private GameObject workspaceMax;
    private GameObject workspaceMin;
    public Material workspaceWarning;
    public Material workspaceMaterial;
    public TextMeshPro titleText;
    public TextMeshPro descriptionText;
    public TextMeshPro buttonText;
    public GameObject dialogWarning;
    private string robot = "";
    public GameObject dialogWelcome;
    public GameObject dialogReferential;
    private float max_WS;
    private float min_WS;

    void Start()
    {
        sphere.GetComponent<Renderer>().material.color = Color.red;
        recognizer = new GestureRecognizer();
        recognizer.SetRecognizableGestures(GestureSettings.Tap);
        recognizer.TappedEvent += GestureRecognizer_TappedEvent;
        recognizer.StartCapturingGestures();
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out MixedRealityPose pose))
        {
            // recalculate sphere position and the finger relative position
            sphere.transform.position = pose.Position;
            relativePosition = getRelativePosition(gizmo.transform, pose.Position);
            coordinates.text = relativePosition.ToString();

            if (recording)
            {
                // update lists
                UpdateLine(pose.Position);
                relativeCoordinates.Add(relativePosition);

                double distanceSphere = Math.Sqrt(Math.Pow(relativePosition.x, 2) + Math.Pow(relativePosition.y, 2) + Math.Pow(relativePosition.z, 2));
                double distanceCylinder = Math.Sqrt(Math.Pow(relativePosition.x, 2) + Math.Pow(relativePosition.z, 2));

                if (robot == "UR5")
                {
                    if (distanceSphere > max_WS / 2 || distanceCylinder < min_WS / 2)
                    {
                        workspaceMax.GetComponent<Renderer>().material = workspaceWarning;
                        workspaceMin.GetComponent<Renderer>().material = workspaceWarning;
                        warning();
                    }
                    else
                    {
                        workspaceMax.GetComponent<Renderer>().material = workspaceMaterial;
                        workspaceMin.GetComponent<Renderer>().material = workspaceMaterial;
                    }
                } 
                else if(robot == "ABB")
                {
                    if (distanceSphere > max_WS / 2 || distanceSphere < min_WS / 2)
                    {
                        workspaceMax.GetComponent<Renderer>().material = workspaceWarning;
                        workspaceMin.GetComponent<Renderer>().material = workspaceWarning;
                        warning();
                    }
                    else
                    {
                        workspaceMax.GetComponent<Renderer>().material = workspaceMaterial;
                        workspaceMin.GetComponent<Renderer>().material = workspaceMaterial;
                    }
                } 
            }
        }
    }

    public void change2Green() // RECORDING
    {
        sphere.GetComponent<Renderer>().material.color = Color.green;
        recording = true;
        CreateLine();
    }

    void change2Red() // NOT RECORDING
    {
        sphere.GetComponent<Renderer>().material.color = Color.red;
        recording = false;
    }

    void OnDestroy()
    {
        recognizer.TappedEvent += GestureRecognizer_TappedEvent;
    }

    private void GestureRecognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        if (begin)
        {
            count++;
            Debug.Log($"Tap - count: {count}");
            if (count % 2 == 0 && count > 0)
            {
                showDialog();
                recognizer.StopCapturingGestures();
            }
            else
            {
                change2Green();
            }
        }
    }

    public void showDialog()
    {
        dialogConfirmation.SetActive(true);
        change2Red();
    }

    public void repeat()
    {
        Debug.Log("REPEAT");
        dialogConfirmation.SetActive(false);
        count = 0;
        lineRenderer.positionCount = 0;
        relativeCoordinates.Clear();
        recognizer.StartCapturingGestures();
    }

    public void send()
    {
        Debug.Log("SEND");
        dialogConfirmation.SetActive(false);
        count = 0;
        lineRenderer.positionCount = 0;
        gesturesPublisher.send2ROS(relativeCoordinates);
        relativeCoordinates.Clear();
        recognizer.StartCapturingGestures();
    }

    private void warning()
    {
        Debug.Log("Warning");
        dialogWarning.SetActive(true);
        change2Red();
        titleText.text = "Warning";
        descriptionText.text = "You exited the robot workspace area, start recording again and try to stay within the green limit.";
        buttonText.text = "Start Recording";
        recognizer.StopCapturingGestures();
    }

    public void restartRecording()
    {
        dialogWarning.SetActive(false);
        count = 0;
        lineRenderer.positionCount = 0;
        relativeCoordinates.Clear();
        recognizer.StartCapturingGestures();
        workspaceMax.GetComponent<Renderer>().material = workspaceMaterial;
        workspaceMin.GetComponent<Renderer>().material = workspaceMaterial;
    }

    private void CreateLine()
    {
        fingerPositions.Clear();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
    }

    private void UpdateLine(Vector3 newFingerPose)
    {
        fingerPositions.Add(newFingerPose);
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newFingerPose);
    }

    public void beginGame()
    {
        begin = true;
        if (robot == "UR5") // UR5 recommended workspace
        {
            max_WS = 1.700f;
            min_WS = 0.151f;
            drawUR5Workspace(max_WS, min_WS, 0.8105f);
        } 
        else if(robot == "ABB")
        {
            max_WS = 2.900f;
            min_WS = 0.940f;
            drawABBWorkspace(max_WS, min_WS);
        }

    }

    public static Vector3 getRelativePosition(Transform origin, Vector3 position)
    {
        Vector3 distance = position - origin.position;
        Vector3 relativePosition2 = Vector3.zero;
        relativePosition2.x = -Vector3.Dot(distance, origin.right.normalized);
        relativePosition2.y = Vector3.Dot(distance, origin.up.normalized);
        relativePosition2.z = Vector3.Dot(distance, origin.forward.normalized);

        return relativePosition2;
    }

    public void drawUR5Workspace(float max, float min, float height)
    {
        workspaceMax = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        workspaceMax.transform.position = gizmo.transform.position;
        workspaceMax.transform.localScale = new Vector3(max, max, max);

        workspaceMin = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        workspaceMin.transform.position = gizmo.transform.position;
        workspaceMin.transform.localScale = new Vector3(min, height, min);

        workspaceMax.GetComponent<Renderer>().material = workspaceMaterial;
        workspaceMin.GetComponent<Renderer>().material = workspaceMaterial;
    }

    public void drawABBWorkspace(float max, float min)
    {
        workspaceMax = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        workspaceMax.transform.position = gizmo.transform.position;
        workspaceMax.transform.localScale = new Vector3(max, max, max);

        workspaceMin = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        workspaceMin.transform.position = gizmo.transform.position;
        workspaceMin.transform.localScale = new Vector3(min, min, min);

        workspaceMax.GetComponent<Renderer>().material = workspaceMaterial;
        workspaceMin.GetComponent<Renderer>().material = workspaceMaterial;
    }

    public void robotName(string name)
    {
        robot = name;
        Debug.Log(robot);
        dialogWelcome.SetActive(false);
        dialogReferential.SetActive(true);
    }
}