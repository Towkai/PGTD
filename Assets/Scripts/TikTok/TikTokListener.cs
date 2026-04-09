using System;
using TikTokLiveUnity;
using UnityEngine;

public class TikTokListener : MonoBehaviour
{
    public string userName = string.Empty;

    #if ODIN_INSPECTOR && UNITY_EDITOR
    [Sirenix.OdinInspector.Button]
    private void ConnectToTikTokBtn()
    {
        if (!Application.isPlaying)
            return;
        ConnectToTikTok();
    }
    #endif
    async void ConnectToTikTok()
    {
        // 連線
        await TikTokLiveManager.Instance.Connect(userName, null);

        // 留言
        var client = TikTokLiveManager.Instance;

        // 留言
        client.OnChatMessage += (sender, e) =>
        {
            Debug.Log($"<color=yellow>{Newtonsoft.Json.JsonConvert.SerializeObject(e)}</color>");
            Debug.Log($"[{ConvertDateTime(e.ClientSendTime)}]{e.Sender.NickName}: {e.Message}");
        };

        // 按讚
        client.OnLike += (sender, e) =>
        {
            Debug.Log($"<color=yellow>{Newtonsoft.Json.JsonConvert.SerializeObject(e)}</color>");
            Debug.Log($"[{ConvertDateTime(e.ClientSendTime)}]{e.Sender.NickName} liked x{e.Count}");
        };

        // 禮物
        client.OnGift += (sender, e) =>
        {
            Debug.Log($"<color=yellow>{Newtonsoft.Json.JsonConvert.SerializeObject(e)}</color>");
            Debug.Log($"{e.Sender.NickName} sent {e.Gift.Name}");
        };

        // 進場
        client.OnJoin += (sender, e) =>
        {
            Debug.Log($"<color=yellow>{Newtonsoft.Json.JsonConvert.SerializeObject(e)}</color>");
            Debug.Log($"[{ConvertDateTime(e.ClientSendTime)}] {e.User.NickName} joined");
        };
    }
    public static string ConvertDateTime(long timestamp)  
    {
        if (timestamp == 0)
            return DateTime.Now.ToLocalTime().ToString();
        DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime;
        return dateTime.ToLocalTime().ToString();;
    }  
}