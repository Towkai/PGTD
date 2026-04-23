using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System;
using System.Net;
using UnityEngine.Events;

namespace Network
{
    public enum ERole { server, host, client }
    public class NetworkLauncher : MonoBehaviour
    {
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.InlineEditor(Sirenix.OdinInspector.InlineEditorObjectFieldModes.Foldout)]
#endif
        [SerializeField] private NetworkAddressConfig config;
        
        private string ip = "127.0.0.1";
        private bool isIpPass = true;
        private ushort port = 7777;
        private bool isPortPass = true;
        public bool isTryingConnect = false;
        private ERole m_role = ERole.client;
        [SerializeField] private UnityEvent OnConnectStart = null;
        [SerializeField] private UnityEvent OnConnectSuccess = null;
        [SerializeField] private UnityEvent OnConnectCancel = null;

        private void OnClientConnected(ulong clientId)
        {
            Debug.Log($"Client connected: {clientId}");
            isTryingConnect = false;
            if (NetworkManager.Singleton.ConnectedClients.Count > 1)
            {
                OnConnectSuccess.Invoke();
                if (NetworkManager.Singleton.IsServer)
                    NetworkManager.Singleton.SceneManager.LoadScene(Data.ConstString.Scene.InGame, UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
        }

        private void OnClientDisconnected(ulong clientId)
        {
            Debug.Log($"Client disconnected: {clientId}");
            if (NetworkManager.Singleton.IsServer && NetworkManager.Singleton.ConnectedClients.Count <= 1)
                NetworkManager.Singleton.SceneManager.LoadScene(Data.ConstString.Scene.Login, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
        void Start()
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }
        public void Connect()
        {
            if (!isIpPass || !isPortPass)
                return;
            if (isTryingConnect)
            {
                NetworkManager.Singleton.Shutdown();
                OnConnectCancel.Invoke();
                isTryingConnect = false;
            }
            else
            {
                Debug.Log("[Netcode]Start Connect");
                switch (m_role)
                {
                    case ERole.host:
                        StartHost();
                        break;
                    case ERole.client:
                        StartClient();
                        break;
                }
                OnConnectStart.Invoke();
                isTryingConnect = true;
            }
        }

        public void SetRole(int value)
        {
            m_role = (ERole)value;
            Data.PlayerPrefsHelper.SetInt(Data.DataKey.last_role_int, value);
        }
        public void SetIPAddress(TMPro.TMP_InputField tmp_Field)
        {
            isIpPass = IPAddress.TryParse(tmp_Field.text, out IPAddress address);
            if (isIpPass)
                ip = address.ToString();
            else
                OnInputError(tmp_Field.textComponent);
        }
        public void SetPort(TMPro.TMP_InputField tmp_Field)
        {
            isPortPass = ushort.TryParse(tmp_Field.text, out port);
            if (!isPortPass)
                OnInputError(tmp_Field.textComponent);
        }
        public void OnInputError(TMPro.TMP_Text text)
        {
            text.color = Color.red;
        }
        public void OnInputEnter(TMPro.TMP_InputField tmp_Field)
        {
            tmp_Field.textComponent.color = Color.Lerp(Color.black, Color.white, 50f/255);
        }
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
            transport.SetConnectionData(ip, port);

            NetworkManager.Singleton.StartClient();
            Debug.Log("Start Client");
        }
    }
}