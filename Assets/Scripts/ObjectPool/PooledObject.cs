using UnityEngine;
using EventDispatcher;
using Interfaces;

namespace ObjectPool
{
    public class PooledObject : MonoBehaviour
    {
        public Pool Pool { get; set; }
        void OnEnable()
        {
            Dispatcher.Instance.Subscribe<EmptyEvent>(ReturnToPool);
            Dispatcher.Instance.Subscribe<RecycleEventArg>(ReturnToPool);
        }

        void OnDisable()
        {
            Dispatcher.Instance.Unsubscribe<EmptyEvent>(ReturnToPool);
            Dispatcher.Instance.Unsubscribe<RecycleEventArg>(ReturnToPool);
        }

        private void ReturnToPool(EmptyEvent e)
        {
            Pool?.Return(this);
        }
        private void ReturnToPool(RecycleEventArg e)
        {
            if (e.Transform == this.transform)
                Pool?.Return(this);
        }
        public void ReturnToPool()
        {
            Pool?.Return(this);
        }

    }
}