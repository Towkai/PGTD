using System;
using System.Collections.Generic;
using UnityEngine;
namespace Data
{
    public static class PlayerPrefsHelper
    {
        public static T GetValue<T>(DataKey key, T defaultValue = default)
        {
            string k = key.ToString();
            System.Type t = typeof(T);

            if (t == typeof(int))
                return (T)(object)PlayerPrefs.GetInt(k, System.Convert.ToInt32(defaultValue));

            else if (t == typeof(float))
                return (T)(object)PlayerPrefs.GetFloat(k, System.Convert.ToSingle(defaultValue));

            else if (t == typeof(string))
                return (T)(object)PlayerPrefs.GetString(k, defaultValue as string);

            else if (t == typeof(bool))
                return (T)(object)(PlayerPrefs.GetInt(k, System.Convert.ToBoolean(defaultValue) ? 1 : 0) == 1);

            else
            {
                string json = PlayerPrefs.GetString(k, "");
                if (string.IsNullOrEmpty(json))
                    return defaultValue;
                return JsonUtility.FromJson<T>(json);
            }
        }
        public static object GetValue(DataKey key)
        {
            string k = key.ToString();

            switch (key)
            {
                case DataKey.last_port_int:
                    return PlayerPrefs.GetInt(k);

                case DataKey.last_channel_string:
                    return PlayerPrefs.GetString(k);

                case DataKey.last_ip_string:
                    return PlayerPrefs.GetString(k);

                case DataKey.last_role_int:
                    return PlayerPrefs.GetInt(k);

                default:
                    return null;
            }
        }

        // ===== int =====
        public static void SetInt(DataKey key, int value)
        {
            PlayerPrefs.SetInt(key.ToString(), value);
        }

        public static int GetInt(DataKey key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(key.ToString(), defaultValue);
        }

        // ===== float =====
        public static void SetFloat(DataKey key, float value)
        {
            PlayerPrefs.SetFloat(key.ToString(), value);
        }

        public static float GetFloat(DataKey key, float defaultValue = 0)
        {
            return PlayerPrefs.GetFloat(key.ToString(), defaultValue);
        }

        // ===== string =====
        public static void SetString(DataKey key, string value)
        {
            PlayerPrefs.SetString(key.ToString(), value);
        }

        public static string GetString(DataKey key, string defaultValue = "")
        {
            return PlayerPrefs.GetString(key.ToString(), defaultValue);
        }

        // ===== bool =====
        public static void SetBool(DataKey key, bool value)
        {
            PlayerPrefs.SetInt(key.ToString(), value ? 1 : 0);
        }

        public static bool GetBool(DataKey key, bool defaultValue = false)
        {
            return PlayerPrefs.GetInt(key.ToString(), defaultValue ? 1 : 0) > 0;
        }

        // ===== delete =====
        public static void Delete(DataKey key)
        {
            PlayerPrefs.DeleteKey(key.ToString());
        }
    }
}