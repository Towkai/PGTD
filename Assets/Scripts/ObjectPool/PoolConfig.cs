using UnityEngine;

namespace ObjectPool
{
    public enum ESide {red, blue}
    [CreateAssetMenu(menuName = "Pool/Config")]
    public class PoolConfig : ScriptableObject
    {
        public string key;              // pool key
        public GameObject prefab;
        public int initialSize = 10;
    }
}