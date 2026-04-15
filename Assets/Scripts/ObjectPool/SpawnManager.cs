using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace ObjectPool
{
    public class SpawnManager : NetworkBehaviour
    {
        public static SpawnManager Instance;
        public NetworkObject m_pool = null;

        [SerializeField] private List<PoolConfig> configs;

        private Dictionary<string, Queue<PooledObject>> poolDict = new();

        private void Awake()
        {
            Instance = this;
        }

        public override void OnNetworkSpawn()
        {
            if (!IsServer) return;

            foreach (var config in configs)
            {
                var queue = new Queue<PooledObject>();

                for (int i = 0; i < config.initialSize; i++)
                {
                    var obj = CreateNew(config);
                    obj.gameObject.SetActive(false);
                    queue.Enqueue(obj);
                }

                poolDict[config.key] = queue;
            }
        }

        private PooledObject CreateNew(PoolConfig config)
        {
            GameObject go = Instantiate(config.prefab);
            if (go.TryGetComponent<PooledObject>(out var pooled))
                pooled.SetPool(config.key);
            if (go.TryGetComponent<NetworkObject>(out var netObj))
            {
                netObj.Spawn(true); // Netcode生成
                netObj.TrySetParent(m_pool);
            }

            return pooled;
        }

        // Server 呼叫
        public PooledObject Spawn(string key, Vector3 pos, Quaternion rot)
        {
            if (!IsServer) return null;

            var pool = poolDict[key];

            PoolConfig config = GetConfig(key);
            PooledObject obj = pool.Count > 0
                ? pool.Dequeue()
                : CreateNew(config);

            obj.OnSpawnFromPoolClientRpc(pos, rot);
            if (config.recycleTime > 0)
                obj.StartCoroutine(obj.LifeTimer(config.recycleTime));

            // 同步 Transform
            if (obj.TryGetComponent<NetworkObject>(out var netObj))
                netObj.TrySetParent(m_pool);
            obj.transform.SetPositionAndRotation(pos, rot);

            return obj;
        }
        
        public void ReturnToPool(PooledObject obj)
        {
            if (!IsServer) return;

            obj.OnReturnToPoolClientRpc();

            poolDict[obj.Key].Enqueue(obj);
        }

        private PoolConfig GetConfig(string id)
        {
            return configs.Find(c => c.key == id);
        }
    }
}