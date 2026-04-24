using System;
using Data;
using EventDispatcher;
using Interfaces;
using TikTokLiveSharp.Client;
using TikTokLiveSharp.Events;
using TikTokLiveUnity;
using UnityEngine;
using UnityEngine.Events;

namespace Live.TikTok 
{
    public class TikTokListener : MonoBehaviour
    {
        public UnityEvent<TikTokLiveClient, Chat> onChat;
        public UnityEvent<TikTokLiveClient, Like> onLike;
        public UnityEvent<TikTokLiveClient, TikTokLiveSharp.Events.Objects.TikTokGift> onGift;
        public UnityEvent<TikTokLiveClient, Join> onJoin;
        public SpawnEventArg spawnEventArg;
        
#if ODIN_INSPECTOR && UNITY_EDITOR
    System.Collections.Generic.List<Sirenix.OdinInspector.ValueDropdownItem<string>> ChannelList = new Sirenix.OdinInspector.ValueDropdownList<string>()
    {
        // {"sebaas_hack", "sebaas_hack"},
        {"อาร์มเมอร์", "armmer_taiyang"},
        {"心心🧊ིིིྀ", "xinyu_316"},
        {"KeeL!", "keel_asli2"},
        {"EL RISAS", "elrisas.ff"},
    };
    [Sirenix.OdinInspector.ValueDropdown("ChannelList")]
    [Header("EditorOnly")] [SerializeField] string UserName;
    [Sirenix.OdinInspector.Button]
    async void ConnectToTikTok()
    {
        var client = TikTokLiveManager.Instance;
        if (!Application.isPlaying || string.IsNullOrEmpty(UserName))
            return;
        if (client.Connected) // 如果已連線，斷線後重連
            await client.DisconnectFromLivestream();
        // 連線
        await client.Connect(UserName, null, e => Debug.Log(e.Message));
    }
#endif

        void Start()
        {
            spawnEventArg = new SpawnEventArg(string.Empty, GameManager.Instance.MySide);
            var client = TikTokLiveManager.Instance;
            client.OnChatMessage += OnChatMessageInvoke;
            client.OnLike += OnLikeInvoke;
            client.OnGift += OnGiftInvoke;
            client.OnJoin += OnJoinInvoke;
        }
        void OnDestroy()
        {
            var client = TikTokLiveManager.Instance;
            client.OnChatMessage -= OnChatMessageInvoke;
            client.OnLike -= OnLikeInvoke;
            client.OnGift -= OnGiftInvoke;
            client.OnJoin -= OnJoinInvoke;            
        }
        public void PrintChatMessageLog(TikTokLiveClient sender, Chat e)
        {
            // Debug.Log($"<color=yellow>[OnChat]</color>{Newtonsoft.Json.JsonConvert.SerializeObject(e)}");
            Debug.Log($"<color=yellow>[OnChat]</color>[{ConvertDateTime(e.ClientSendTime)}]{e.Sender.NickName}: {e.Message}");
        }
        private void OnChatMessageInvoke(TikTokLiveClient sender, Chat e)
        {
            onChat.Invoke(sender, e);
        }
        public void PrintLikeLog(TikTokLiveClient sender, Like e)
        {
            // Debug.Log($"<color=yellow>[Onlike]</color>{Newtonsoft.Json.JsonConvert.SerializeObject(e)}");
            Debug.Log($"<color=yellow>[OnLike]</color>[{ConvertDateTime(e.ClientSendTime)}]{e.Sender.NickName} liked x{e.Count}");
        }
        private void OnLikeInvoke(TikTokLiveClient sender, Like e)
        {
            onLike.Invoke(sender, e);
        }
        public void PrintGiftLog(TikTokLiveClient sender, TikTokLiveSharp.Events.Objects.TikTokGift e)
        {
            // Debug.Log($"<color=yellow>{Newtonsoft.Json.JsonConvert.SerializeObject(e)}");
            Debug.Log($"<color=red>[OnGift]</color>{e.Sender.NickName} sent {e.Gift.Name}");
        }
        public void SpawnGiftMinion(TikTokLiveClient sender, TikTokLiveSharp.Events.Objects.TikTokGift e)
        {
            Debug.Log($"<color=red>[OnGift]<color=red>SpawnMinion: {e.Gift.Name} Combo: {e.Gift.Combo}</color>");
            spawnEventArg.SetKey(ConstString.PooledObject.GetMinionKey(e.Gift.Name));
            Dispatcher.Instance.Dispatch(spawnEventArg);
        }

        private void OnGiftInvoke(TikTokLiveClient sender, TikTokLiveSharp.Events.Objects.TikTokGift e)
        {
            onGift.Invoke(sender, e);
        }
        public void PrintJoinLog(TikTokLiveClient sender, Join e)
        {
            // Debug.Log($"<color=yellow>[OnJoin]</color>{Newtonsoft.Json.JsonConvert.SerializeObject(e)}");
            Debug.Log($"<color=yellow>[OnJoin]</color>[{ConvertDateTime(e.ClientSendTime)}] {e.User.NickName} joined");
        }
        private void OnJoinInvoke(TikTokLiveClient sender, Join e)
        {
            onJoin.Invoke(sender, e);
            // spawnEventArg.SetKey(GetMinionKey("Rose"));
            // Dispatcher.Instance.Dispatch(spawnEventArg);
        }
        public static string ConvertDateTime(long timestamp)  
        {
            if (timestamp == 0)
                return DateTime.Now.ToLocalTime().ToString();
            DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime;
            return dateTime.ToLocalTime().ToString();;
        }  
    }
}