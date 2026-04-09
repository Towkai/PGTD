using System;
using TikTokLiveUnity;
using UnityEngine;

namespace LiveInteraction
{
    public class TikTok : LivePlatformBase
    {
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.OnValueChanged("Connect")]
#endif
        public string userName = "";

        public override async void Connect()
        {
#if UNITY_EDITOR
        if (!Application.isPlaying || String.IsNullOrEmpty(userName))
            return;
#endif
            try
            {
                base.Connect();
                Debug.Log("TikTok Connecting...");
                
                await TikTokLiveManager.Instance.Connect(userName, null);

                isConnected = true;

                Debug.Log("TikTok Connected");
            }
            catch (Exception e)
            {
                Debug.LogError("TikTok connect failed: " + e.Message);
            }
        }
        void Start()
        {
                var client = TikTokLiveManager.Instance;
                // 聊天
                client.OnChatMessage += (sender, e) =>
                {
                    string msg = $"[{ConvertTime(e.ClientSendTime)}]{e.Sender.NickName}: {e.Message}";
                    EmitChat(msg);
                };

                // 按讚
                client.OnLike += (sender, e) =>
                {
                    string msg = $"[{ConvertTime(e.ClientSendTime)}]{e.Sender.NickName} liked x{e.Count}";
                    EmitLike(msg);
                };

                // 禮物
                client.OnGift += (sender, e) =>
                {
                    string msg = $"{e.Sender.NickName} sent {e.Gift.Name}";
                    Debug.Log(e);
                    EmitGift(msg);
                };

                // 進場
                client.OnJoin += (sender, e) =>
                {
                    string msg = $"[{ConvertTime(e.ClientSendTime)}]{e.User.NickName} joined";
                    EmitJoin(msg);
                };
        }

        string ConvertTime(long timestamp)
        {
            if (timestamp == 0)
                return DateTime.Now.ToString();

            DateTime dt = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime;
            return dt.ToLocalTime().ToString();
        }
    }
}