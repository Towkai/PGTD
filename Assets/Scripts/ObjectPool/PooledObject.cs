using Unity.Netcode;
using UnityEngine;
using Interfaces;
using EventDispatcher;
using System.Collections;

namespace ObjectPool
{
    public class PooledObject : NetworkBehaviour
    {
        public string Key { get; private set; }
        public bool isActive => this.gameObject.activeInHierarchy;

        public void SetPool(string key)
        {
            this.Key = key;
        }

        // 給 PoolManager 呼叫初始化
        public virtual void OnSpawnFromPool(Vector3 pos, Quaternion rot)
        {
            transform.SetPositionAndRotation(pos, rot);
            gameObject.SetActive(true);
        }

        // 回收
        public virtual void OnReturnToPool()
        {
            gameObject.SetActive(false);
        }

        void OnEnable()
        {
            Dispatcher.Instance.Subscribe<RecycleEventArg>(ReturnToPool);
        }

        void OnDisable()
        {
            Dispatcher.Instance.Unsubscribe<RecycleEventArg>(ReturnToPool);
        }

        public IEnumerator LifeTimer(float t)
        {
            while (t > 0)
            {
                t -= Time.deltaTime;
                yield return null;
            }
            GameManager.Instance.SpawnManager.ReturnToPool(this);
        }

        private void ReturnToPool(RecycleEventArg e)
        {
            if (e.Transform == this.transform)
            {
                e.OnRecycle?.Invoke();
                GameManager.Instance.SpawnManager.ReturnToPool(this);
                // networkObject.Despawn();
            }
        }
        
    }
}