using System;
using UnityEngine;

namespace ObjectPool
{
    [CreateAssetMenu(menuName = "MyConfig/Pool")]
    public class PoolConfig : ScriptableObject
    {
        public string key;              // pool key
        public GameObject prefab;
        [Range(0, Byte.MaxValue)]
        public int initialSize;
    }
}