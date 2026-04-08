using UnityEngine;
using EventDispatcher;
using Interfaces;
using UnityEngine.Events;

namespace ObjectPool
{
    public class PooledObject : MonoBehaviour
    {
        public Pool Pool { get; set; }
        public float recycleTime = -1; //回收時間，在Prefab的Inspector裡設定。-1為不會定時回收
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        private float recycleTimer = 0;
        [SerializeField] private UnityEvent onSpawn, onRecycle;

        void Start()
        {
            onSpawn.AddListener(Init);
        }
        void OnEnable()
        {
            Dispatcher.Instance.Subscribe<RecycleEvent>(ReturnToPool);
            Dispatcher.Instance.Subscribe<RecycleEventArg>(ReturnToPool);
            onSpawn?.Invoke();
        }

        void OnDisable()
        {
            Dispatcher.Instance.Unsubscribe<RecycleEvent>(ReturnToPool);
            Dispatcher.Instance.Unsubscribe<RecycleEventArg>(ReturnToPool);
            onRecycle?.Invoke();
        }
        void Init()
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
                // e.Action?.Invoke();
            }
        }
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.Button]
#endif
        public void ReturnToPool()
        {
            Pool?.Return(this);
            // Dispatcher.Instance.Dispatch(new RecycleEventArg(this.transform));
        }

    }
}