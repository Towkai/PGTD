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
        [SerializeField] private float m_harm; //子彈威力
        [SerializeField] private float m_speed;
        private RecycleEventArg recycleEventArg = null;
        
        void Start()
        {
            m_meshRenderer ??= this.GetComponent<MeshRenderer>();
            m_trailRenderer ??= this.GetComponent<TrailRenderer>();
            recycleEventArg = new RecycleEventArg(this.transform, Print);
        }
        
#if UNITY_EDITOR
        void Print()
        {
            if (m_debug)
                Debug.Log("Debug");
        }
#endif
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