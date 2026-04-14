using UnityEngine;

namespace Network
{
#if UNITY_EDITOR
    public enum net_type { host, client }
#endif
    [CreateAssetMenu(menuName = "MyConfig/IpAddress")]
    public class NetworkAddressConfig : ScriptableObject
    {
        public string ip = "127.0.0.1";
        public ushort port = 7777;

#if UNITY_EDITOR
        [Header("EditorOnly")]
        [SerializeField]
        public net_type debug;
#endif

    }
}
