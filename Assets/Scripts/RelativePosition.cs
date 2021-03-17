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
    public TextMesh xCoord;
    public TextMesh yCoord;
    public TextMesh zCoord;
    public TextMesh timestamp;
    public TextMesh nanoseconds;

    // Update is called once per frame
    void Update()
    {
        long time = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
        timestamp.text = (time).ToString();
        DateTime offsetDate = new DateTime(1970, 1, 1);
        nanoseconds.text = (DateTime.Now.Ticks * 100 - offsetDate.Ticks * 100 - time * 1000000000).ToString();

        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out MixedRealityPose pose))
        {
            handAbsolute.text = "Abs: " + pose.Position.x + ", " + pose.Position.y + ", " + pose.Position.z + "\n";
            Vector3 relativePosition = getRelativePosition(cube.transform, pose.Position);
            handRelative.text = "Rel: " + relativePosition.x + ", " + relativePosition.y + ", " + relativePosition.z + "\n";
            xCoord.text = relativePosition.x.ToString();
            yCoord.text = relativePosition.y.ToString();
            zCoord.text = relativePosition.z.ToString();
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

