using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using Microsoft;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;
using System;

public class RelativePosition : MonoBehaviour
{
    public GameObject cube;
    public TextMesh handAbsolute;
    public TextMesh handRelative;
    public TextMesh timestamp;
    public long seconds;
    public long nanoseconds;
    public Vector3 relativePosition;

    // Update is called once per frame
    void Update()
    {
        seconds = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
        DateTime offsetDate = new DateTime(1970, 1, 1);
        nanoseconds = DateTime.Now.Ticks * 100 - offsetDate.Ticks * 100 - seconds * 1000000000;
        timestamp.text = seconds.ToString() + "." + nanoseconds.ToString();

        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out MixedRealityPose pose))
        {
            handAbsolute.text = "Abs: " + pose.Position.x + ", " + pose.Position.y + ", " + pose.Position.z + "\n";
            relativePosition = getRelativePosition(cube.transform, pose.Position);
            handRelative.text = "Rel: " + relativePosition.x + ", " + relativePosition.y + ", " + relativePosition.z + "\n";
        }
    }

    public static Vector3 getRelativePosition(Transform origin, Vector3 position)
    {
        Vector3 distance = position - origin.position;
        Vector3 relativePosition2 = Vector3.zero;
        relativePosition2.x =   Vector3.Dot(distance, origin.right.normalized);
        relativePosition2.y =   Vector3.Dot(distance, origin.up.normalized);
        relativePosition2.z = - Vector3.Dot(distance, origin.forward.normalized);

        return relativePosition2;
    }
}

