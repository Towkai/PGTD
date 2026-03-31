using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private GameObject prefab;
#if ODIN_INSPECTOR && UNITY_EDITOR
    [Sirenix.OdinInspector.ShowInInspector]
#endif
    private Queue<PooledObject> pool = new Queue<PooledObject>();
    private Transform parent;

    public ObjectPool(GameObject prefab, int initialSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            CreateNew();
        }
    }

    private PooledObject CreateNew()
    {
        GameObject obj = GameObject.Instantiate(prefab, parent);
        obj.SetActive(false);

        var pooled = obj.GetComponent<PooledObject>();
        if (pooled == null)
            pooled = obj.AddComponent<PooledObject>();

        pooled.Pool = this;
        pool.Enqueue(pooled);

        return pooled;
    }

    public PooledObject Get(Vector3 position, Quaternion rotation)
    {
        if (pool.Count == 0)
        {
            CreateNew();
        }

        var obj = pool.Dequeue();
        obj.transform.SetPositionAndRotation(position, rotation);
        obj.gameObject.SetActive(true);

        return obj;
    }

    public void Return(PooledObject obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}