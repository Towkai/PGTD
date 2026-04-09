using System;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

namespace LiveInteraction
{
    public class Twitch : LivePlatformBase
    {
        TcpClient client;
        StreamReader reader;
        StreamWriter writer;

        const string URL = "irc.chat.twitch.tv";
        const int PORT = 6667;

        [SerializeField] string user;
        [SerializeField] string oauth; // get OAuth from https://twitchtokengenerator.com/
#if ODIN_INSPECTOR && UNITY_EDITOR
        System.Collections.Generic.List<Sirenix.OdinInspector.ValueDropdownItem<string>> ChannelList = new Sirenix.OdinInspector.ValueDropdownList<string>()
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
        [Sirenix.OdinInspector.OnValueChanged("Connect")]
#endif
        [SerializeField] string channel;

        [SerializeField] float reconnectDelay = 60f;
        float reconnectTimer = 0;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.ShowInInspector]
        float ReconnectTimer => reconnectTimer;
#endif

        public override void Connect()
        {
#if UNITY_EDITOR
        if (!Application.isPlaying || String.IsNullOrEmpty(channel))
            return;
#endif
            try
            {
                base.Connect();
                Debug.Log("Twitch Connecting...");
                client = new TcpClient(URL, PORT);
                var stream = client.GetStream();

                reader = new StreamReader(stream);
                writer = new StreamWriter(stream);

                writer.WriteLine($"PASS {oauth}");
                writer.WriteLine($"NICK {user.ToLower()}");
                writer.WriteLine("CAP REQ :twitch.tv/tags twitch.tv.commands twitch.tv.membership");
                writer.WriteLine($"JOIN #{channel.ToLower()}");
                writer.Flush();

                isConnected = true;
                reconnectTimer = 0;

                Debug.Log("Twitch Connected");
            }
            catch (System.Exception e)
            {
                Debug.LogError("Twitch connect failed: " + e.Message);
            }
        }

        protected override void Tick()
        {
            if (client == null || !client.Connected)
            {
                reconnectTimer += Time.deltaTime;

                if (reconnectTimer >= reconnectDelay)
                {
                    Debug.Log("Reconnecting Twitch...");
                    Connect();
                    reconnectTimer = 0;
                }
                return;
            }

            while (client.Available > 0)
            {
                string msg = reader.ReadLine();

                if (string.IsNullOrEmpty(msg)) return;

                // PING 防斷線
                if (msg.StartsWith("PING"))
                {
                    writer.WriteLine("PONG :tmi.twitch.tv");
                    writer.Flush();
                    return;
                }

                HandleMessage(msg);
            }
        }

        void HandleMessage(string message)
        {
            // 聊天
            if (message.Contains("PRIVMSG"))
            {
                // string user = GetUser(message);
                string text = GetMessage(message);

                EmitChat(text);
            }
            // 訂閱 / 禮物
            else if (message.Contains("USERNOTICE"))
            {
                string type = GetTagValue(message, "msg-id");
                string user = GetTagValue(message, "display-name");

                switch (type)
                {
                    case "sub":
                        Debug.Log($"{user} 訂閱");
                        // EmitGift();
                        break;

                    case "resub":
                        Debug.Log($"{user} 續訂");
                        // EmitGift();
                        break;

                    case "subgift":
                        Debug.Log($"{user} 贈送訂閱");
                        // EmitGift();
                        break;

                    case "submysterygift":
                        Debug.Log($"{user} 大量送訂閱");
                        // EmitGift();
                        break;
                }
            }
        }

        #region Parsing
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
            var match = System.Text.RegularExpressions.Regex.Match(message, @"PRIVMSG\s+#\w+\s+:(.*)");
            return match.Success ? match.Groups[1].Value : "";
        }

        string GetTagValue(string message, string key)
        {
            if (!message.StartsWith("@")) return null;

            int end = message.IndexOf(' ');
            string tags = message.Substring(1, end - 1);

            foreach (var pair in tags.Split(';'))
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