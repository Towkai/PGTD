using System;
using UnityEngine;

namespace Interfaces
{
    public interface IEvent { }
    public struct RecycleEvent : IEvent {}
    public class RecycleEventArg : IEvent
    {
        Transform m_transform;
        // Action m_Action;
        public Transform Transform => m_transform;
        // public Action Action => m_Action;
        public RecycleEventArg(Transform transform)
        {
            this.m_transform = transform;
        }
        // public RecycleEventArg(Transform transform, Action action)
        // {
        //     this.m_transform = transform;
        //     this.m_Action = action;
        // }
    }
}
