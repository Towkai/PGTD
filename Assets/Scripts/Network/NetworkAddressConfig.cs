using Unity.Netcode;
using UnityEngine;

namespace Network
{
#if UNITY_EDITOR
#endif
    [CreateAssetMenu(menuName = "MyConfig/IpAddress")]
    public class NetworkAddressConfig : ScriptableObject
    {
        public string ip = "127.0.0.1";
        public ushort port = 7777;

#if UNITY_EDITOR
        [Header("EditorOnly")]
        [SerializeField]
        public ERole debug;
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void StartHost()
        {
            var transport = NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>();
            transport.SetConnectionData("0.0.0.0", port);

            NetworkManager.Singleton.StartHost();
            Debug.Log("Start Host");
        }
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void StartClient()
        {
            var transport = NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>();
            transport.SetConnectionData(ip, port);

            NetworkManager.Singleton.StartClient();
            Debug.Log("Start Client");
        }
#endif

    }
}
