using System;
using UnityEngine;

namespace Interfaces
{
    public interface IEvent { }
    public struct RecycleEvent : IEvent {}
    public struct RecycleEventArg : IEvent
    {
        Transform m_transform;
        Action m_Action;
        public Transform Transform => m_transform;
        public Action Callback => m_Action;
        public RecycleEventArg(Transform transform, Action callback = null)
        {
            this.m_transform = transform;
            this.m_Action = callback;
        }
    }
    public struct ChangeImageEventArg : IEvent
    {
        UnityEngine.UI.Image m_image;
        Data.MinionType m_minionType;
        Action m_Action;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        public UnityEngine.UI.Image Image => m_image;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        public Data.MinionType MinionType => m_minionType;
        public Action Callback => m_Action;
        public ChangeImageEventArg(UnityEngine.UI.Image image, Data.MinionType minionType = Data.MinionType.Cube, Action callback = null)
        {
            this.m_image = image;
            this.m_minionType = minionType;
            this.m_Action = callback;
        }
        public void SetImage(UnityEngine.UI.Image image)
        {
            this.m_image = image;
        }
        public void SetMinionType(Data.MinionType type)
        {
            this.m_minionType = type;
        }
    }
}
