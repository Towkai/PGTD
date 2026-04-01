using UnityEngine;
using EventDispatcher;
using Interfaces;

namespace ObjectPool
{
    public struct RecycleEvent : IEvent {}
    public class PooledObject : MonoBehaviour
    {
        public Pool Pool { get; set; }
        void OnEnable()
        {
            Dispatcher.Instance.Subscribe<RecycleEvent>(ReturnToPool);
        }

        void OnDisable()
        {
            Dispatcher.Instance.Unsubscribe<RecycleEvent>(ReturnToPool);
        }

        private void ReturnToPool(RecycleEvent e)
        {
            Pool?.Return(this);
        }

    }
}