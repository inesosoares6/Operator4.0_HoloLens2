using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusPublisher : RosSharp.RosBridgeClient.Publisher<RosSharp.RosBridgeClient.Messages.Standard.String>
{
    private RosSharp.RosBridgeClient.Messages.Standard.String message;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        message = new RosSharp.RosBridgeClient.Messages.Standard.String();
    }

    public void status2ROS(string status)
    {
        message.data = status;
        Publish(message);
    }
}
