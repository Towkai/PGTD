using Unity.Netcode;
using UnityEngine;

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
    }
}