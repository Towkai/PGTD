using System;
using EventDispatcher;
using Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Setting
{
    public class SelectMinion : MonoBehaviour
    {
        // private string minionKeyBase = "{0:00}.TikTok.{1}.{2}"; //{0}: 怪物編號: 01~10、{1}: Red/Blue、{2}: Cube/Cone/Cylinder
        string select_num; //正在設定的編號(1~10)
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        ChangeImageEventArg changeImageEventArg = new ChangeImageEventArg(null);
        public void SetNowSelect(Button button)
        {
            changeImageEventArg.SetImage(button.image);
            this.select_num = button.name.Split('.')[0];
        }

        public void SetMinionType(int type)
        {
            SetMinionType((Data.MinionType)type);
        }
        void SetMinionType(Data.MinionType type)
        {
            string playerFabsKey = string.Format(Data.ConstString.PooledObject.MinionType, select_num);
            changeImageEventArg.SetMinionType(type);
            Apply(playerFabsKey, type);
        }
        public void Apply(string key, Data.MinionType type)
        {
            if (Enum.TryParse<Data.DataKey>(key, out var result))
            {
                Data.PlayerPrefsHelper.SetString(result, type.ToString());            
                Dispatcher.Instance.Dispatch(changeImageEventArg);
            }
            else
                Debug.Log("[Unknow Data Key]: " + key);
        }
    }
}