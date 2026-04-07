using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool
{
    public class Pool
    {
        private GameObject prefab;
    #if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.ShowInInspector]
    #endif
        private Queue<PooledObject> pool = new Queue<PooledObject>();
        
        private Vector3[] posRange;
        public Vector3 Position => new Vector3(Random.Range(posRange[0].x, posRange[1].x), Random.Range(posRange[0].y, posRange[1].y), Random.Range(posRange[0].z, posRange[1].z));
        private Quaternion rotation;
        private Transform parent;

        public Pool(GameObject prefab, int initialSize, Vector3[] posRange, Vector3 eulerRotation, Transform parent = null)
        {
            this.prefab = prefab;
            this.parent = parent;
            this.posRange = posRange;
            this.rotation = Quaternion.Euler(eulerRotation);

            for (int i = 0; i < initialSize; i++)
            {
                CreateNew(Position, rotation);
            }
        }

        private PooledObject CreateNew(Vector3 pos, Quaternion rot)
        {
            GameObject obj = GameObject.Instantiate(prefab, pos, rot, parent);
            obj.SetActive(false);

            var pooled = obj.GetComponent<PooledObject>();
            if (pooled == null)
                pooled = obj.AddComponent<PooledObject>();

            pooled.Pool = this;
            pool.Enqueue(pooled);

            return pooled;
        }

        public PooledObject Get(Vector3? pos, Quaternion? rot)
        {
            Vector3 vector3 = pos ?? Position;
            Quaternion quaternion = rot == null ? rotation : rotation * (Quaternion)rot;
            
            if (pool.Count == 0)
            {
                CreateNew(vector3, quaternion);
            }

            var obj = pool.Dequeue();
            obj.transform.SetPositionAndRotation(vector3, quaternion);
            obj.gameObject.SetActive(true);

            return obj;
        }

        public void Return(PooledObject obj)
        {
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }
}