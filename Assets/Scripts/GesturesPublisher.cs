using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class GesturesPublisher : Publisher<Messages.Geometry.Vector3>
    {
        private Messages.Geometry.Vector3 message;

        // Start is called before the first frame update
        void Start()
        {
            base.Start();
            message = new Messages.Geometry.Vector3();
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
}