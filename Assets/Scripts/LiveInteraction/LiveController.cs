using UnityEngine;
using UnityEngine.Events;

namespace LiveInteraction
{
    public class LiveController : MonoBehaviour
    {
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.OnValueChanged("Connect")]
#endif
        [SerializeField] LivePlatformBase platform;
        public UnityEvent<string> onChat;
        public UnityEvent<string> onLike;
        public UnityEvent<string> onGift;
        public UnityEvent<string> onJoin;

#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        public bool IsConnected => platform?.IsConnected ?? false;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.Button]
#endif
        protected void Connect()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif
            platform.SetEventListener(onChat, onLike, onGift, onJoin);
            platform.Connect();
        }
        void Start()
        {
            onChat.AddListener(msg => Debug.Log("CHAT: " + msg));
            onGift.AddListener(msg => Debug.Log("GIFT: " + msg));
            onLike.AddListener(msg => Debug.Log("LIKE: " + msg));
            onJoin.AddListener(msg => Debug.Log("JOIN: " + msg));
        }
    }
}