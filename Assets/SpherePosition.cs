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
    public GameObject sphere;
    public GameObject dialog;
    private GestureRecognizer recognizer;
    private int count;

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
        }
    }

    void change2Green()
    {
        sphere.GetComponent<Renderer>().material.color = Color.green;
    }

    void change2Red()
    {
        sphere.GetComponent<Renderer>().material.color = Color.red;
    }

    void OnDestroy()
    {
        recognizer.TappedEvent += GestureRecognizer_TappedEvent;
    }

    private void GestureRecognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        count++;
        Debug.Log($"Tap - count: {count}");
        if(count%2 == 0 && count!=0)
        {
            showDialog();
        }
        else
        {
            change2Green();
        }
    }

    public void showDialog()
    {
        dialog.SetActive(true);
    }

    public void repeat()
    {
        Debug.Log("REPEAT");
        dialog.SetActive(false);
        change2Green();
    }

    public void send()
    {
        Debug.Log("SEND");
        dialog.SetActive(false);
        change2Red();
        count = 0;
    }
}
