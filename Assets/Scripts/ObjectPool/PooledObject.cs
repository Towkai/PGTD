using UnityEngine;
using EventDispatcher;
using Interfaces;

namespace ObjectPool
{
    public class PooledObject : MonoBehaviour
    {
        public Pool Pool { get; set; }
        public float recycleTime = -1; //回收時間，在Prefab的Inspector裡設定。-1為不會定時回收
        [SerializeField] private float recycleTimer = 0;

        void OnEnable()
        {
            Dispatcher.Instance.Subscribe<RecycleEvent>(ReturnToPool);
            Dispatcher.Instance.Subscribe<RecycleEventArg>(ReturnToPool);
        }

        void OnDisable()
        {
            Dispatcher.Instance.Unsubscribe<RecycleEvent>(ReturnToPool);
            Dispatcher.Instance.Unsubscribe<RecycleEventArg>(ReturnToPool);
        }
        void Reset()
        {
            recycleTimer = 0;
        }
        void Update()
        {
            if (recycleTime < 0)
                return;
            recycleTimer += Time.deltaTime;
            if (recycleTimer > recycleTime)
                ReturnToPool();
        }

        private void ReturnToPool(RecycleEvent e)
        {
            Pool?.Return(this);
        }
        private void ReturnToPool(RecycleEventArg e)
        {
            if (e.Transform == this.transform)
            {
                Pool?.Return(this);
                e.Action?.Invoke();
            }
        }
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.Button]
#endif
        public void ReturnToPool()
        {
            Pool?.Return(this);
            Reset();
            // Dispatcher.Instance.Dispatch(new RecycleEventArg(this.transform));
        }

    }
}