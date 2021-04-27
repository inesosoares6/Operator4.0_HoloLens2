using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using Microsoft;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;
using System;
using Microsoft.MixedReality.Toolkit.UI;

public class SpherePosition : MonoBehaviour
{
    private bool begin = false;
    public GameObject sphere;
    public GameObject dialog;
    private GestureRecognizer recognizer;
    private int count;
    private bool recording = false;
    private LineRenderer lineRenderer;
    private List<Vector3> fingerPositions = new List<Vector3>();
    public GameObject gizmo;
    public Vector3 relativePosition;
    public TextMesh coordinates;

    void Start()
    {
        sphere.GetComponent<Renderer>().material.color = Color.red;
        recognizer = new GestureRecognizer();
        recognizer.SetRecognizableGestures(GestureSettings.Tap);
        recognizer.TappedEvent += GestureRecognizer_TappedEvent;
        recognizer.StartCapturingGestures();
        //recognizer.StopCapturingGestures();
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out MixedRealityPose pose))
        {
            sphere.transform.position = pose.Position;
            relativePosition = getRelativePosition(gizmo.transform, pose.Position);
            coordinates.text = relativePosition.ToString();
            if (recording)
            {
                UpdateLine(pose.Position);
            }
        }
    }

    void change2Green() // RECORDING
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
        dialog.SetActive(true);
        change2Red();
    }

    public void repeat()
    {
        Debug.Log("REPEAT");
        dialog.SetActive(false);
        change2Green();
        lineRenderer.positionCount = 0;
        recognizer.StartCapturingGestures();
    }

    public void send()
    {
        Debug.Log("SEND");
        dialog.SetActive(false);
        count = 0;
        lineRenderer.positionCount = 0;
        recognizer.StartCapturingGestures();
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
    }

    public static Vector3 getRelativePosition(Transform origin, Vector3 position)
    {
        Vector3 distance = position - origin.position;
        Vector3 relativePosition2 = Vector3.zero;
        relativePosition2.x = - Vector3.Dot(distance, origin.right.normalized);
        relativePosition2.y = Vector3.Dot(distance, origin.up.normalized);
        relativePosition2.z = Vector3.Dot(distance, origin.forward.normalized);

        return relativePosition2;
    }
}
