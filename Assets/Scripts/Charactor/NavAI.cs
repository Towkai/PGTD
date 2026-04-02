using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Charactor
{
    public class NavAI : MonoBehaviour
    {
        private Transform m_target;
        public Coroutine ChasingCoroutine = null;
        [SerializeField]
        private NavMeshAgent m_navAgent;
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f); //不要每幀執行
// #region ODIN_Button
//         #if ODIN_INSPECTOR && UNITY_EDITOR

//         [Sirenix.OdinInspector.Button("StartChasingTarget")]
//         private void StartChasingTargetButton()
//         {
//             if (m_target != null && ChasingCoroutine == null)
//                 StartCoroutine(StartChasingTarget());
//         }
//         #endif
// #endregion
        void Start()
        {
            if (waitForSeconds == null)
                waitForSeconds = new WaitForSeconds(0.1f);
            if (m_navAgent == null)
                m_navAgent = this.GetComponent<NavMeshAgent>();
            if (m_target != null && ChasingCoroutine == null)
                ChasingCoroutine = StartCoroutine(StartChasingTarget());
        }
        public void SetTarget(Transform newTarget)
        {
            m_target = newTarget;
            if (ChasingCoroutine == null)
                ChasingCoroutine = StartCoroutine(StartChasingTarget());
        }
        private IEnumerator StartChasingTarget()
        {
            while (m_target != null)
            {
                m_navAgent.SetDestination(m_target.position);
                yield return waitForSeconds;
            }
            m_navAgent.speed = 0;
            ChasingCoroutine = null;
        }
    }
}