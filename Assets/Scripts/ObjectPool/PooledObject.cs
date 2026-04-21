using Unity.Netcode;
using UnityEngine;
using Interfaces;
using EventDispatcher;
using System.Collections;
using UnityEngine.Events;

namespace ObjectPool
{
    public class PooledObject : NetworkBehaviour
    {
        [SerializeField] protected Renderer[] m_renderers = null;
        public string Key { get; private set; }
        public bool isActive => this.gameObject.activeInHierarchy;
        public UnityEvent onSpawn = null;
        public UnityEvent onRecycle = null;
        public void SetPool(string key)
        {
            this.Key = key;
        }
        // 給 PoolManager 呼叫初始化
        [ClientRpc]
        public virtual void OnSpawnFromPoolClientRpc(Vector3 pos, Quaternion rot)
        {
            transform.SetPositionAndRotation(pos, rot);
            gameObject.SetActive(true);
            Dispatcher.Instance.Subscribe<RecycleEvent>(ReturnToPool);
            Dispatcher.Instance.Subscribe<RecycleEventArg>(ReturnToPool);
            onSpawn.Invoke();
        }
        // 回收
        [ClientRpc]
        public virtual void OnReturnToPoolClientRpc()
        {
            Dispatcher.Instance.Unsubscribe<RecycleEvent>(ReturnToPool);
            Dispatcher.Instance.Unsubscribe<RecycleEventArg>(ReturnToPool);
            gameObject.SetActive(false);
            onRecycle.Invoke();
        }
        protected override void OnNetworkPreSpawn(ref NetworkManager networkManager)
        {
            base.OnNetworkPreSpawn(ref networkManager);
        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
        }
        public void SetRenderer(bool enabled)
        {
            if (m_renderers == null || m_renderers.Length == 0)
                return;
            for (int i = 0; i < m_renderers.Length; i++)
                m_renderers[i].enabled = enabled;
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
        private void ReturnToPool(RecycleEvent e)
        {
            GameManager.Instance.SpawnManager.ReturnToPool(this);
        }
        private void ReturnToPool(RecycleEventArg e)
        {
            if (e.Transform == this.transform)
            {
                Debug.Log("RecycleEventArg");
                e.Callback?.Invoke();
                GameManager.Instance.SpawnManager.ReturnToPool(this);
                // networkObject.Despawn();
            }
        }
    }
}