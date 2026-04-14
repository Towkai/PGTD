using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

namespace Network
{
    public class NetworkLauncher : MonoBehaviour
    {
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.InlineEditor(Sirenix.OdinInspector.InlineEditorObjectFieldModes.Foldout)]
#endif
        public NetworkAddressConfig config;
#if UNITY_EDITOR
        public void Start()
        {
            switch (config.debug)
            {
                case net_type.host:
                    StartHost();
                    break;
                case net_type.client:
                    StartClient();
                    break;
            }
        }
#endif
        public void StartHost()
        {
            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.SetConnectionData("0.0.0.0", config.port);

            NetworkManager.Singleton.StartHost();
            Debug.Log("Start Host");
        }

        public void StartClient()
        {
            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.SetConnectionData(config.ip, config.port);

            NetworkManager.Singleton.StartClient();
            Debug.Log("Start Client");
        }
    }
}