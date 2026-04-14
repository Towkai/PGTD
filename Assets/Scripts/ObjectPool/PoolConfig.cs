using UnityEngine;

namespace ObjectPool
{
    [CreateAssetMenu(menuName = "MyConfig/Pool")]
    public class PoolConfig : ScriptableObject
    {
        public string key;              // pool key
        public GameObject prefab;
        public int initialSize = 10;
    }
}