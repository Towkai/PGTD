using System;
using TikTokLiveUnity;
using UnityEngine;
using UnityEngine.Events;

public class TikTokListener : MonoBehaviour
{
#if ODIN_INSPECTOR && UNITY_EDITOR
    [Sirenix.OdinInspector.ShowInInspector]
#endif
    public string UserName => tmp_Field.text;
    public UnityEvent onConnectStart;
    public UnityEvent<bool> onConnectSuccess;
    public UnityEvent<Exception> onConnectFail;
    public TMPro.TMP_InputField tmp_Field;
#if ODIN_INSPECTOR && UNITY_EDITOR
    [Sirenix.OdinInspector.Button]
    private void ConnectToTikTokBtn()
    {
        if (!Application.isPlaying)
            return;
        ConnectToTikTok();
    }
#endif
    public void Start()
    {
        onConnectFail.AddListener((e) => tmp_Field.textComponent.color = Color.red);
        tmp_Field.onValueChanged.AddListener((e) => tmp_Field.textComponent.color = Color.Lerp(Color.black, Color.white, 50f/255));
    }
    public void OnConnectBtnClick() //在Connect按鈕Inspector中設定
    {
        onConnectStart.Invoke();
        ConnectToTikTok();
    }
    public void OnConnectSuccess(bool value) //在TikTokLiveManager的Inspector中設定
    {
        onConnectSuccess.Invoke(value);
    }
    async void ConnectToTikTok()
    {
        if (string.IsNullOrEmpty(UserName))
        {
            onConnectFail.Invoke(null);
            return;
        }
        // 連線
        await TikTokLiveManager.Instance.Connect(UserName, null, e => onConnectFail.Invoke(e));

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