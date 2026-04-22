using Network;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Common
{
    public class DefaultValue : MonoBehaviour
    {
        public Data.DataKey key = Data.DataKey.none;
        public UnityEvent startEvent = null;

        void Start()
        {
            startEvent.Invoke();
        }
        public static void GetLastChannel(TMPro.TMP_InputField inputField)
        {
            inputField.text = Data.PlayerPrefsHelper.GetString(Data.DataKey.last_channel_string);
        }
        public static void GetLastIp(TMPro.TMP_InputField inputField)
        {
            var ip = Data.PlayerPrefsHelper.GetString(Data.DataKey.last_ip_string);
            inputField.text = string.IsNullOrEmpty(ip) ? "127.0.0.1" : ip;            
        }
        public static void GetLastPort(TMPro.TMP_InputField inputField)
        {
            var port = Data.PlayerPrefsHelper.GetInt(Data.DataKey.last_port_int);
            inputField.text = port == 0 ? "7777" : port.ToString(); 
        }

        public void GetLastRole(int role)
        {
            this.GetComponent<Toggle>().isOn = Data.PlayerPrefsHelper.GetInt(key) == role;
        }

        public void SetInt(TMPro.TMP_InputField inputField)
        {
            if (int.TryParse(inputField.text, out var i))
                Data.PlayerPrefsHelper.SetInt(key, i);
            else
                Debug.LogError("ErrorType");
        }
        public void SetFloat(TMPro.TMP_InputField inputField)
        {
            if (float.TryParse(inputField.text, out var f))
                Data.PlayerPrefsHelper.SetFloat(key, f);
            else
                Debug.LogError("ErrorType");
        }
        public void SetString(TMPro.TMP_InputField inputField)
        {
            Data.PlayerPrefsHelper.SetString(key, inputField.text);
        }
        public void SetBool(TMPro.TMP_InputField inputField)
        {
            if (bool.TryParse(inputField.text, out var b))
                Data.PlayerPrefsHelper.SetBool(key, b);
            else if (int.TryParse(inputField.text, out var i))
                Data.PlayerPrefsHelper.SetBool(key, i > 0);
            else
                Debug.LogError("ErrorType");
        }
    }
}