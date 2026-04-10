using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

namespace Network
{
    public class NetworkLauncher : MonoBehaviour
    {
        public string ip = "127.0.0.1";
        public ushort port = 7777;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.Button]
#endif
        public void StartHost()
        {
            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.SetConnectionData("0.0.0.0", port);

            NetworkManager.Singleton.StartHost();
            Debug.Log("Start Host");
        }

#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.Button]
#endif
        public void StartClient()
        {
            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.SetConnectionData(ip, port);

            NetworkManager.Singleton.StartClient();
            Debug.Log("Start Client");
        }

    }
}