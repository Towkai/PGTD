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
        // public RecycleEventArg(Transform transform, Action action)
        // {
        //     this.m_transform = transform;
        //     this.m_Action = action;
        // }
    }
}
