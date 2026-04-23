using System;
using EventDispatcher;
using Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Setting
{
    public class MinionImageController : MonoBehaviour
    {
        public Image image = null;
        public Sprite[] minionSprites = new Sprite[Enum.GetNames(typeof(Data.MinionType)).Length];
        public Data.MinionType type = Data.MinionType.Cube;
        void Reset()
        {
            image ??= this.GetComponent<Image>();
        }
        void OnEnable()
        {
            string number = this.name.Split('.')[0];
            string dataKey = string.Format(Data.ConstString.PooledObject.MinionTypeKey, number);
            if (Enum.TryParse<Data.DataKey>(dataKey, out var result)) //取出每種圖案代表的Key(n{xx}_minion_type)
            {
                string dataValue = Data.PlayerPrefsHelper.GetString(result);
                if (Enum.TryParse<Data.MinionType>(dataValue, out type)) //取出每種圖案代表的value(Cube/Cone/Cylinder)
                    this.image.sprite = minionSprites[(int)type];
                else
                    Debug.Log("[Unknow Data Key]: " + dataValue);
            }
            else
                Debug.Log("[Unknow Data Key]: " + dataKey);        
        }
        void Start()
        {
            Dispatcher.Instance.Subscribe<ChangeImageEventArg>(ChangeImage);
        }
        void OnDestroy()
        {
            Dispatcher.Instance.Unsubscribe<ChangeImageEventArg>(ChangeImage);
        }
        private void ChangeImage(ChangeImageEventArg e)
        {
            if (this.image == e.Image)
            {
                e.Callback?.Invoke();
                type = e.MinionType;
                e.Image.sprite = minionSprites[(int)e.MinionType];
            }
        }
    }
}