using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool
{
    public class SpawnManager : MonoBehaviour
    {
        public static SpawnManager Instance;

        public List<PoolConfig> poolConfigs;

        private Dictionary<string, Pool> pools = new Dictionary<string, Pool>();

        void Awake()
        {
            Instance = this;

            foreach (var config in poolConfigs)
            {
                pools[config.key] = new Pool(
                    config.prefab,
                    config.initialSize,
                    config.posRange,
                    config.eulerRot,
                    this.transform
                );
            }
        }

        public GameObject Spawn(string key)
        {
            if (!pools.ContainsKey(key))
            {
                Debug.LogError($"Pool not found: {key}");
                return null;
            }

            var pooled = pools[key].Get(null, null);
            return pooled.gameObject;
        }
    }
}