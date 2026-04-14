using System;
using UnityEngine;

namespace ObjectPool
{
    [CreateAssetMenu(menuName = "MyConfig/Pool")]
    public class PoolConfig : ScriptableObject
    {
        public string key;              // pool key
        public GameObject prefab;
        public float recycleTime = -1; //回收時間，在Prefab的Inspector裡設定。-1為不會定時回收
        [Range(0, Byte.MaxValue)]
        public int initialSize;
    }
}