using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using Microsoft;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;

namespace RosSharp.RosBridgeClient
{
    public class PositionPublisher : Publisher<Messages.Geometry.PoseStamped>
    {
        private Messages.Geometry.PoseStamped message;
        private bool buttonSend = false;
        private uint i;
        public RelativePosition handPosition;

        // Start is called before the first frame update
        void Start()
        {
            base.Start();
            message = new Messages.Geometry.PoseStamped();
            i = 0;
        }

        private void Update()
        {
            if (buttonSend)
            {
                message.header.stamp.secs = (uint) handPosition.seconds;
                message.header.stamp.nsecs = (uint) handPosition.nanoseconds;
                message.pose.position.x = handPosition.relativePosition.x;
                message.pose.position.y = handPosition.relativePosition.y;
                message.pose.position.z = handPosition.relativePosition.z;
                //message.pose.orientation.x = 0;
                //message.pose.orientation.y = 0;
                //message.pose.orientation.z = 0;
                //message.pose.orientation.z = 0;
                message.header.seq = i;
                message.header.frame_id = "world";
                Publish(message);
                i++;
            }
        }

        public void SendButtonTrue() // function called from OnClick() in ButtonSend
        {
            buttonSend = true;
        }

        public void StopButtonTrue() // function called from OnClick() in ButtonStop
        {
            buttonSend = false;
        }
    }
}