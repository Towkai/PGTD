using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Events;

namespace Live.Twitch
{
    public class TwitchConnect : MonoBehaviour
    {
        TcpClient Twitch;
        StreamReader Reader;
        StreamWriter Writer;

        const string URL = "irc.chat.twitch.tv";
        const int PORT = 6667;

        [SerializeField] 
        float reconnectDelay = 60;
        float reconnectTimer = 0;
        [SerializeField] 
        string User = "pagui_aki";
        [SerializeField] // get OAuth from https://twitchtokengenerator.com/
        string OAuth = "oauth:rn9mokua611pk0a82ex217kwcpe7lh";
        #if ODIN_INSPECTOR && UNITY_EDITOR
        List<Sirenix.OdinInspector.ValueDropdownItem<string>> ChannelList = new Sirenix.OdinInspector.ValueDropdownList<string>()
        {
            {"pagui_aki", "pagui_aki"},
            {"ksonsouchou", "ksonsouchou"},
            {"兎寒うさむ", "usamu_1127"},
            {"火暴可可", "xhibaoger"},
            {"嬌兔", "zrush"},
            {"懶貓", "failverde"},
            {"蘇雪霏", "sharronsu_ch"},
        };
        [Sirenix.OdinInspector.ValueDropdown("ChannelList")]
        [Sirenix.OdinInspector.OnValueChanged("ConnectToTwitchBtn")]
        #endif
        [SerializeField] 
        string Channel = "ksonsouchou";
        [SerializeField]
        private UnityEvent<string> on_chat, on_sub, on_resub, on_subgift, on_submysterygift;

        #if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.Button]
        private void ConnectToTwitchBtn()
        {
            if (!Application.isPlaying)
                return;
            ConnectToTwitch();
            reconnectTimer = 0;
        }
        #endif
        private void ConnectToTwitch()
        {
            try
            {
                Twitch = new TcpClient(URL, PORT);
                var stream = Twitch.GetStream();
                Reader = new StreamReader(stream);
                Writer = new StreamWriter(stream);

                Writer.WriteLine($"PASS {OAuth}");
                Writer.WriteLine($"NICK {User.ToLower()}");

                Writer.WriteLine("CAP REQ :twitch.tv/tags twitch.tv.commands twitch.tv.membership");

                Writer.WriteLine($"JOIN #{Channel.ToLower()}");
                Writer.Flush();

                Debug.Log("Connected to Twitch");
            }
            catch (System.Exception e)
            {
                Debug.LogError("Connect failed: " + e.Message);
            }
        }

        // void Awake()
        // {
        //     ConnectToTwitch();
        // }
        

        // Update is called once per frame
        void Update()
        {
            // 重連機制
            if (Twitch == null || !Twitch.Connected)
            {
                reconnectTimer += Time.deltaTime;
                if (reconnectTimer >= reconnectDelay)
                {
                    Debug.Log("Reconnecting...");
                    ConnectToTwitch();
                    reconnectTimer = 0;
                }
                return;
            }
            // 讀取訊息
            while (Twitch.Available > 0)
            {
                string message = Reader.ReadLine();
                if (string.IsNullOrEmpty(message)) return;

                Debug.Log($"<color=yellow>{message}</color>");

                // 回應 PING 防斷線
                if (message.StartsWith("PING"))
                {
                    Writer.WriteLine("PONG :tmi.twitch.tv");
                    Writer.Flush();
                    return;
                }

                HandleMessage(message);
            }
        }

        void HandleMessage(string message)
        {
            // 聊天訊息
            if (message.Contains("PRIVMSG"))
            {
                string user = GetUser(message);
                string msg = GetMessage(message);

                Debug.Log($"{user}: {msg}");
                on_chat?.Invoke(msg);            
            }

            // 訂閱 / 贈送事件
            else if (message.Contains("USERNOTICE"))
            {
                string msgId = GetTagValue(message, "msg-id");
                string user = GetTagValue(message, "display-name");

                switch (msgId)
                {
                    case "sub":
                        Debug.Log($"{user} 訂閱");
                        on_sub?.Invoke(message);
                        break;

                    case "resub":
                        Debug.Log($"{user} 續訂");
                        on_resub?.Invoke(message);
                        break;

                    case "subgift":
                        string recipient = GetTagValue(message, "msg-param-recipient-display-name");
                        Debug.Log($"{user} 贈送訂閱給 {recipient}");
                        on_subgift?.Invoke(message);
                        break;

                    case "submysterygift":
                        string count = GetTagValue(message, "msg-param-mass-gift-count");
                        Debug.Log($"{user} 送了 {count} 個訂閱");
                        on_submysterygift?.Invoke(message);
                        break;
                }
            }
            else
            {
                Debug.LogWarning("<color=red>未定義事件。</color>");
            }
        }

    #region Parsing 工具
        string GetUser(string message)
        {
            try
            {
                int excl = message.IndexOf('!');
                int start = message.LastIndexOf(':', excl) + 1;
                return message.Substring(start, excl - start);
            }
            catch { return "Unknown"; }
        }

        string GetMessage(string message)
        {
            // 提取訊息內容
            string pattern = @"PRIVMSG\s+#\w+\s+:(.*)";
            System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(message, pattern);

            if (match.Success && match.Groups.Count > 1)
            {
                return match.Groups[1].Value;
            }
            else
                return "<color=red>Message Parse Fail</color>";
        }

        string GetTagValue(string message, string key)
        {
            if (!message.StartsWith("@")) return null;

            int end = message.IndexOf(' ');
            string tags = message.Substring(1, end - 1);
            string[] pairs = tags.Split(';');

            foreach (var pair in pairs)
            {
                var kv = pair.Split('=');
                if (kv[0] == key)
                    return kv.Length > 1 ? kv[1] : "";
            }

            return null;
        }
        #endregion
    }
}