using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool
{
    [CreateAssetMenu(menuName = "MyConfig/PoolGroup")]
    public class PoolConfigGroup : ScriptableObject
    {
        [SerializeField] private List<PoolConfig> m_configs;
        public List<PoolConfig> Configs => m_configs;

    }
}