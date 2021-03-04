/*
© Siemens AG, 2017-2018
Author: Dr. Martin Bischoff (martin.bischoff@siemens.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
<http://www.apache.org/licenses/LICENSE-2.0>.
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    [RequireComponent(typeof(RosConnector))]
    public abstract class Subscriber<T> : MonoBehaviour where T: Message
    {
        public string Topic;
        public float TimeStep;
        string subscription_id=null;

        protected virtual void Start()
        {
            //subscription_id = GetComponent<RosConnector>().RosSocket.Subscribe<T>(Topic, ReceiveMessage, (int)(TimeStep * 1000)); // the rate(in ms in between messages) at which to throttle the topics
        }

        private void OnEnable()
        {
            subscription_id = GetComponent<RosConnector>().RosSocket.Subscribe<T>(Topic, ReceiveMessage, (int)(TimeStep * 1000)); // the rate(in ms in between messages) at which to throttle the topics
        }

        private void OnDisable()
        {

            RosSocket socket = GetComponent<RosConnector>().RosSocket;
            if (subscription_id!=null && socket!=null)
                socket.Unsubscribe(subscription_id);
        }

        protected abstract void ReceiveMessage(T message);

    }
}