using UnityEngine;
using EventDispatcher;
using Interfaces;
using System.Collections;

namespace Character
{
    public class Bullet : MonoBehaviour
    {
#if UNITY_EDITOR
        public bool m_debug;
#endif
        [SerializeField] private LayerMask m_enemyLayer;
        [SerializeField] private MeshRenderer m_meshRenderer;
        [SerializeField] private TrailRenderer m_trailRenderer;
        [SerializeField] private float m_trailTime = 0.5f;
        [SerializeField] private float m_harm; //子彈威力
        [SerializeField] private float m_speed;
        private RecycleEventArg recycleEventArg = null;
        
        public void init() //由PooledObject呼叫初始化
        {
            recycleEventArg ??= new RecycleEventArg(this.transform);
        }

        void Update()
        {
            this.transform.position += this.transform.forward * m_speed;
        }
        void OnTriggerExit(Collider other)
        {
            if (IsInLayerMask(other.gameObject))
            {
                other.GetComponent<CharacterBase>().GetInjured(m_harm);
                Dispatcher.Instance.Dispatch(recycleEventArg);
            }
        }
        public bool IsInLayerMask(GameObject obj)
        {
            return (m_enemyLayer.value | (1 << obj.layer)) == m_enemyLayer;
        }
    }
}