using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System;

namespace ObjectPool
{
    public class NetworkLauncher : MonoBehaviour
    {
        public string ip = "127.0.0.1";
        public ushort port = 7777;

#if ODIN_INSPECTOR && UNITY_EDITOR
        enum net_type { host, client }
        [SerializeField]
        net_type cnt;
        void Start()
        {
            switch (cnt)
            {
                case net_type.host:
                    StartHost();
                    break;
                case net_type.client:
                    StartClient();
                    break;
            }        }
#endif

        public void StartHost()
        {
            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.SetConnectionData("0.0.0.0", port);

            NetworkManager.Singleton.StartHost();
            Debug.Log("Start Host");
        }

        public void StartClient()
        {
            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.SetConnectionData(ip, port);

            NetworkManager.Singleton.StartClient();
            Debug.Log("Start Client");
        }

    }
}