using System.Collections;
using UnityEngine;
using EventDispatcher;
using Interfaces;
using Unity.VisualScripting;

namespace Charactor
{
    public class Minion : MonoBehaviour
    {
        [SerializeField] private LayerMask m_enemyLayer;
        [SerializeField] private NavAI m_nav;
        [SerializeField] private int m_fullBlood;
        [SerializeField] private int m_nowBlood;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.ShowInInspector]
        [Sirenix.OdinInspector.ReadOnly]
#endif
        private Transform mainTarget;

#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.OnValueChanged("SetNavTarget")]
#endif
        [SerializeField] private Transform m_target;

#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        public Transform Target => m_target == null || m_target.Equals(null) || !m_target.gameObject.activeInHierarchy ? mainTarget : m_target;
#region 搜敵功能
        public float detectionRadius = 3.5f; // 檢測半徑 
        public float attackRadius = 2f; // 攻擊半徑 
        private Coroutine SearchTargetCoroutine = null, AttackTargetCoroutine = null;
        private WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f); //不要每幀執行
#endregion
        private RecycleEventArg recycleEventArg = null;
        void Reset()
        {
            m_nowBlood = m_fullBlood;
        }
        void OnEnable()
        {
            Reset();         
        }
        void Start()
        {
            recycleEventArg = new RecycleEventArg(this.transform);
            mainTarget = GameObject.Find(LayerMask.LayerToName((int)Mathf.Log(m_enemyLayer.value, 2))).transform;
            SetNavTarget();
        }
        void Update()
        {
#if UNITY_EDITOR
            if (m_target != null)
                Debug.DrawLine(this.transform.position, m_target.position, Color.blue);
#endif
        }
#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            // 假设您在检测 radius 为 5 的球形区域
            if (SearchTargetCoroutine != null)
                Gizmos.DrawWireSphere(this.transform.position, detectionRadius);
        }
#endif
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.Button]
#endif
        private void SetNavTarget()
        {
            m_nav.SetTarget(Target);
            if (m_target == null)
                StartSearchEnemy();
        }
        public void StartSearchEnemy()
        {
            if (SearchTargetCoroutine == null)
                SearchTargetCoroutine = StartCoroutine(GetClosestEnemy());
        }
        public void GetHarm(int value)
        {
            m_nowBlood -= value;
        }
        IEnumerator GetClosestEnemy()
        {
            while (true)
            {
                // 1. 在位置處產生球形檢測
                Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, detectionRadius, m_enemyLayer);
                if (hitColliders.Length > 0)
                {
                    if (hitColliders.Length == 1)
                        m_target = hitColliders[0].transform;
                    else
                    {
                        Collider closest = null;
                        float minDistance = Mathf.Infinity;
                        // 遍歷檢測到的碰撞器
                        foreach (Collider collider in hitColliders)
                        {
                            float distance = Vector3.Distance(transform.position, collider.transform.position);
                            
                            // 找出距離最近的
                            if (distance < minDistance)
                            {
                                minDistance = distance;
                                closest = collider;
                            }
                        }
                        m_target = closest.transform;
                    }
                    SetNavTarget();
                    break;
                }
                else
                {
                    yield return waitForSeconds;
                }
            }
            SearchTargetCoroutine = null;
        }
        IEnumerator GetTargetDist()
        {
            if (Target != null)
            {
                float dist = Vector3.Distance(this.transform.position, Target.transform.position);
                
            }
        }
        void OnCollisionEnter(Collision collision)
        {
            if (m_enemyLayer == 1 << collision.gameObject.layer)
            {
                if (collision.transform.tag == "Minion")
                    Dispatcher.Instance.Dispatch(recycleEventArg);
            }
        }
    }
}