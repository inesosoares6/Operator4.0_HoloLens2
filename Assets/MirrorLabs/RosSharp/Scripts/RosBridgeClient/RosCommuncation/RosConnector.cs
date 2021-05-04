/*
© Siemens AG, 2017
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

using System;
using System.Collections;
using System.Threading;
using RosSharp.RosBridgeClient.Protocols;
using UnityEngine;
using UnityEngine.Events;

namespace RosSharp.RosBridgeClient
{
    public class RosConnector : MonoBehaviour
    {
        public int Timeout = 10;

        public RosSocket RosSocket { get; private set; }
        public enum Protocols { WebSocketSharp, WebSocketNET, WebSocketUWP };
        public RosBridgeClient.RosSocket.SerializerEnum Serializer;
        public Protocols Protocol;
        private string RosBridgeServerUrl;
        private string RosBridgeServer_Prefix = "ws://";
        public string RosBridgeServer_IP = "172.16.48.104";
        public string RosBridgeServer_Port = "9090";

        private ManualResetEvent isConnected = new ManualResetEvent(false);    

        public void ConnectDisconnect()
        {
            if (this.enabled)
                this.enabled = false;
            else
                this.enabled = true;
        }

        private void OnEnable()
        {
            if (RosSocket != null)
                RosSocket.Close();
            RosBridgeServerUrl = RosBridgeServer_Prefix + RosBridgeServer_IP + ":" + RosBridgeServer_Port;
            new Thread(ConnectAndWait).Start();
        }

        private void OnDisable()
        {
            this.gameObject.GetComponent<GesturesPublisher>().enabled = true;
            
            if (RosSocket != null)
                RosSocket.Close();

        }
        public void GUI_SetPort(string port)
        {
            RosBridgeServer_Port = port;
            RosBridgeServerUrl = RosBridgeServer_Prefix + RosBridgeServer_IP + ":" + RosBridgeServer_Port;
        }
        public void GUI_SetIP(string ip)
        {
            RosBridgeServer_IP = ip;
            RosBridgeServerUrl = RosBridgeServer_Prefix + RosBridgeServer_IP + ":" + RosBridgeServer_Port;
        }
        public void DispatchToMainThread_RosConnectionSuccess()
        {
            UnityMainThreadDispatcher.Instance().Enqueue(DispactchedRosConnectionSuccess());
        }
        public IEnumerator DispactchedRosConnectionSuccess()
        {
            yield return new WaitForSeconds(1);
            RosConnectionSuccess();
            yield return null;
        }
        private void RosConnectionSuccess()
        {
            this.gameObject.GetComponent<GesturesPublisher>().enabled = true;
        }

        public void DispatchToMainThread_RosConnectionDisconnected()
        {
            UnityMainThreadDispatcher.Instance().Enqueue(DispactchedRosConnectionDisconnected());
        }
        public IEnumerator DispactchedRosConnectionDisconnected()
        {
            yield return new WaitForSeconds(1);
            RosConnectionDisconnected();
            yield return null;
        }
        private void RosConnectionDisconnected()
        {
        }


        // --------------------------------------------------------------------------------------------------


        /* public void Awake()
         {

 #if WINDOWS_UWP
             ConnectAndWait();
 #else
             new Thread(ConnectAndWait).Start();
 #endif
         }*/

        private void ConnectAndWait()
        {
            RosSocket = ConnectToRos(Protocol, RosBridgeServerUrl, OnConnected, OnClosed,Serializer);

            if (!isConnected.WaitOne(Timeout * 1000))
                Debug.LogWarning("Failed to connect to RosBridge at: " + RosBridgeServerUrl);            
        }
        
        public static RosSocket ConnectToRos(Protocols protocolType, string serverUrl, EventHandler onConnected = null, EventHandler onClosed = null,RosSocket.SerializerEnum serializer=RosSocket.SerializerEnum.JSON)
        {
            RosBridgeClient.Protocols.IProtocol protocol = GetProtocol(protocolType, serverUrl);
            protocol.OnConnected += onConnected;
            protocol.OnClosed += onClosed;

            return new RosSocket(protocol,serializer);
        }

        private static RosBridgeClient.Protocols.IProtocol GetProtocol(Protocols protocol, string rosBridgeServerUrl)
        {

#if WINDOWS_UWP
                return new RosBridgeClient.Protocols.WebSocketUWPProtocol(rosBridgeServerUrl);
#else
            switch (protocol)
            {
                case Protocols.WebSocketNET:
                    return new RosBridgeClient.Protocols.WebSocketNetProtocol(rosBridgeServerUrl);
                case Protocols.WebSocketSharp:
                    return new RosBridgeClient.Protocols.WebSocketSharpProtocol(rosBridgeServerUrl);
                case Protocols.WebSocketUWP:
                    Debug.Log("WebSocketUWP only works when deployed to HoloLens, defaulting to WebSocketNetProtocol");
                    return new RosBridgeClient.Protocols.WebSocketNetProtocol(rosBridgeServerUrl);
                default:
                    return null;
            }
#endif
        }

        private void OnApplicationQuit()
        {
            if(RosSocket!=null)
                RosSocket.Close();
        }

        private void OnConnected(object sender, EventArgs e)
        {
            isConnected.Set();            
            Debug.Log("Connected to RosBridge: " + RosBridgeServerUrl);

            try
            {
                DispatchToMainThread_RosConnectionSuccess();
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }

        private void OnClosed(object sender, EventArgs e)
        {
            isConnected.Reset();            
            Debug.Log("Disconnected from RosBridge: " + RosBridgeServerUrl);

            try
            {
                DispatchToMainThread_RosConnectionDisconnected();
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }   
        }
    }
}
