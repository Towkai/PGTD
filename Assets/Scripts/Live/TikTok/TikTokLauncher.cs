using System;
using TikTokLiveUnity;
using UnityEngine;
using UnityEngine.Events;

namespace Live.TikTok 
{
    public class TikTokLauncher : MonoBehaviour
    {
        public string UserName => tmp_Field?.text;
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
        void Start()
        {
            if (onConnectFail != null)
                onConnectFail.AddListener((e) => tmp_Field.textComponent.color = Color.red);
            if (tmp_Field != null)
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
            var client = TikTokLiveManager.Instance;
            if (client.Connected) // 如果已連線，斷線後重連
                await client.DisconnectFromLivestream();
        
            await client.Connect(UserName, null, e => onConnectFail.Invoke(e)); // 連線
        }
    }
}