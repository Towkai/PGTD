using UnityEngine;

namespace Interfaces
{
    public interface IEvent { }
    public struct EmptyEvent : IEvent {}
    public class RecycleEventArg : IEvent
    {
        Transform m_transform;
        public Transform Transform => m_transform;
        public RecycleEventArg(Transform transform)
        {
            this.m_transform = transform;
        }
    }
}
