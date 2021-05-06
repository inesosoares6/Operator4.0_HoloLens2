﻿using System.Collections.Generic;
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
    public GameObject dialog;
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
    public GameObject workspaceMax;
    public GameObject workspaceMin;
    public Material workspaceWarning;
    public Material workspaceMaterial;
    public TextMeshPro titleText;
    public TextMeshPro descriptionText;
    public TextMeshPro buttonText;
    public GameObject dialogWarning;

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
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out MixedRealityPose pose) && recording)
        {
            // recalculate sphere position and the finger relative position
            sphere.transform.position = pose.Position;
            relativePosition = getRelativePosition(gizmo.transform, pose.Position);
            coordinates.text = relativePosition.ToString();

            // update lists
            UpdateLine(pose.Position);
            relativeCoordinates.Add(relativePosition);

            double distance = Math.Sqrt(Math.Pow(relativePosition.x, 2) + Math.Pow(relativePosition.y, 2) + Math.Pow(relativePosition.z, 2));
            if (distance > 1.7 || relativePosition.x < 0.151 || relativePosition.z < 0.151)
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
        relativeCoordinates.Clear();
        recognizer.StartCapturingGestures();
    }

    public void send()
    {
        Debug.Log("SEND");
        dialog.SetActive(false);
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
        count = 0;
        lineRenderer.positionCount = 0;
        relativeCoordinates.Clear();
    }

    public void restartRecording()
    {
        dialogWarning.SetActive(false);
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
        drawRobotWorkspace();
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

    public void drawRobotWorkspace()
    {
        workspaceMax.SetActive(true);
        workspaceMax.transform.position = gizmo.transform.position;

        workspaceMin.SetActive(true);
        workspaceMin.transform.position = gizmo.transform.position;
    }
}