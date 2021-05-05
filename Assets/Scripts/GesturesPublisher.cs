using System.Collections.Generic;
using UnityEngine;

public class GesturesPublisher : RosSharp.RosBridgeClient.Publisher<RosSharp.RosBridgeClient.Messages.Geometry.Vector3>
{
    private RosSharp.RosBridgeClient.Messages.Geometry.Vector3 message;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        message = new RosSharp.RosBridgeClient.Messages.Geometry.Vector3();
    }

    public void send2ROS(List<Vector3> relativeCoordinates)
    {
        while (relativeCoordinates.Count!=0)
        {
            message.x = relativeCoordinates[0].x;
            message.y = relativeCoordinates[0].y;
            message.z = relativeCoordinates[0].z;
            Publish(message);
            relativeCoordinates.RemoveAt(0);
        }
    }
}