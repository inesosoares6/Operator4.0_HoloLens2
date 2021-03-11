﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;
using System;
using System.Reflection;

public class SetReferentialManager : MonoBehaviour
{
    private int _count;
    public GameObject itemToPosition;
    public GameObject anchorScriptReference;
    private Vector3 origin;
    private Vector3 coordXaxis;
    private Vector3 coordZaxis;
    private float timer = 0.0f;
    private bool buttonConfirm = false;
    private Vector3 lookPosition, lookVector;
    private Quaternion lookRotation;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("\nCurrentCount: "+ CurrentCount);
    }

    public int CurrentCount
    {
        get { return _count; }
        set
        {
            if (value != _count)
            {
                // Ensure current index isn't more/less than the index bounds
                if (_count >= 0 || _count <= 4)
                {
                    _count = value;
                    Debug.Log("CurrentCount: " + CurrentCount);
                }
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        switch (CurrentCount)
        {
            // Define referetial origin
            case 1:
                if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out MixedRealityPose pose1) && buttonConfirm)
                {
                    timer += Time.deltaTime;
                    if (timer > 5.0f)
                    {
                        origin = pose1.Position;
                        Debug.Log("origin:" + origin + " count:" + CurrentCount);
                        CurrentCount++;
                        timer = 0.0f;
                        buttonConfirm = false;
                    }
                }
                break;

            // Define point in X axis
            case 2:
                if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out MixedRealityPose pose2) && buttonConfirm)
                {
                    timer += Time.deltaTime;
                    if (timer > 5.0f)
                    {
                        coordXaxis = pose2.Position;
                        Debug.Log("coordXaxis:" + coordXaxis + " count:" + CurrentCount);
                        CurrentCount++;
                        timer = 0.0f;
                        buttonConfirm = false;
                    }
                }
                break;

            // Define point in Y axis
            case 3:
                if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out MixedRealityPose pose3) && buttonConfirm)
                {
                    timer += Time.deltaTime;
                    if (timer > 5.0f)
                    {
                        coordZaxis = pose3.Position;
                        Debug.Log("coordZaxis:" + coordZaxis + " count:" + CurrentCount);
                        CurrentCount++;
                        timer = 0.0f;
                        buttonConfirm = false;
                    }
                }
                break;

            // Define orientation and save anchor
            case 4:
                // delete previous anchor
                anchorScriptReference.GetComponent<AnchorScript>().DestroyExistingAnchor();
                if (buttonConfirm)
                {
                    CurrentCount = 0;
                    buttonConfirm = false;

                    // define referential origin (coordinations for the cube)
                    itemToPosition.transform.position = new Vector3(origin.x, origin.y, origin.z);

                    // calculate orientation
                    //lookPosition.x = (float) (Math.Sqrt(Math.Pow(coordXaxis.x,2) + Math.Pow(coordXaxis.z,2)));
                    //lookPosition.z = (float) (Math.Sqrt(Math.Pow(coordZaxis.x,2) + Math.Pow(coordZaxis.z,2)));
                    lookPosition.x = coordZaxis.x;
                    lookPosition.z = coordZaxis.z;
                    lookPosition.y = (coordXaxis.y + coordZaxis.y) / 2;

                    // set referential orientation
                    lookVector = itemToPosition.transform.position - lookPosition;
                    lookRotation = Quaternion.LookRotation(lookVector, Vector3.up);
                    itemToPosition.transform.rotation = lookRotation;

                    // save anchor
                    anchorScriptReference.GetComponent<AnchorScript>().SaveAnchor();
                    Debug.Log("Anchor saved\n");
                }
                break;
        }
    }

    // Button to define coordinate is checked
    public void ConfirmButtonTrue() // function called from OnClick() in ButtonConfirm
    {
        buttonConfirm = true;
    }
}