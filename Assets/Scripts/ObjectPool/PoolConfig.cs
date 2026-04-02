using UnityEngine;

namespace ObjectPool
{
    [CreateAssetMenu(menuName = "Pool/Config")]
    public class PoolConfig : ScriptableObject
    {
        public string key;              // pool key
        public GameObject prefab;
        public int initialSize = 10;
        [SerializeField]
        public Vector3[] posRange = new Vector3[2];
        public Vector3 eulerRot;
    }
}