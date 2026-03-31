using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [System.Serializable]
    public class PoolConfig
    {
        public string key;
        public GameObject prefab;
        public int initialSize = 10;
    }

    public List<PoolConfig> poolConfigs;

    private Dictionary<string, ObjectPool> pools = new();

    void Awake()
    {
        Instance = this;

        foreach (var config in poolConfigs)
        {
            pools[config.key] = new ObjectPool(
                config.prefab,
                config.initialSize,
                this.transform
            );
        }
    }

    public GameObject Spawn(string key, Vector3 position, Quaternion rotation)
    {
        if (!pools.ContainsKey(key))
        {
            Debug.LogError($"Pool not found: {key}");
            return null;
        }

        var pooled = pools[key].Get(position, rotation);
        return pooled.gameObject;
    }
}