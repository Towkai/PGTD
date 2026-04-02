using System.Collections;
using UnityEngine;
using EventDispatcher;
using Interfaces;
using Unity.VisualScripting;

namespace Charactor
{
    public class Minion : MonoBehaviour
    {
        enum State
        {
            idle, //m_target為空的狀態(往敵方主塔前進)
            alert, //m_target有東西的狀態(往敵方小怪前進的狀態)
            attack //進入攻擊範圍的狀態
        }
        private State m_state = State.idle;
        [SerializeField] private LayerMask m_enemyLayer;
        [SerializeField] private NavAI m_nav;
        [SerializeField] private int m_fullBlood;
        [SerializeField] private int m_nowBlood;
        private Transform mainTarget;
        private Transform m_target;
        public Transform Target => m_target == null || m_target.Equals(null) || !m_target.gameObject.activeInHierarchy ? mainTarget : m_target;

#region 搜敵功能
        public float detectionRadius = 3.5f; // 檢測半徑 
        public float attackRadius = 2f; // 攻擊半徑 
#endregion
        private RecycleEventArg recycleEventArg = null;
        void Start()
        {
            recycleEventArg = new RecycleEventArg(this.transform);
            mainTarget = GameObject.Find(LayerMask.LayerToName((int)Mathf.Log(m_enemyLayer.value, 2))).transform;        
        }
        void Update()
        {
            switch (m_state)
            {
                case State.idle:
                    break;
                case State.alert:
                    break;
                case State.attack:
                    break;
            }
        }
    }
}