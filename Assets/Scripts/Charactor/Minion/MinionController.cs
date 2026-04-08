using System.Collections;
using EventDispatcher;
using Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace Character
{
    public class MinionController : CharacterBase
    {
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.ShowInInspector]
        private string CurrentState => StateMachine?.CurrentState.id ?? "";
#endif
        public StateMachine StateMachine { get; private set; }
        public MinionNormalState NormalState { get; private set; }
        public MinionChaseState ChaseState { get; private set; }
        public MinionAttackState AttackState { get; private set; }
        [SerializeField] private NavMeshAgent m_navAgent;
        [SerializeField] private float m_speed;
        public float detectRange = 3.5f; //搜敵範圍
        public float attackRange = 2f; //攻擊範圍
        public float attackDelay = 1f; //攻擊間距
        public LayerMask enemyLayer;
        private RecycleEventArg recycleEventArg = null;

        private Transform main_target, m_target;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        public Transform Target { 
            get => m_target == null || m_target.Equals(null) || !m_target.gameObject.activeInHierarchy ? (main_target ??= GameObject.Find(LayerMask.LayerToName((int)Mathf.Log(enemyLayer.value, 2))).transform) : m_target;
            set => m_target = value;
        }
        public void SetNavDestination()
        {
            m_navAgent.SetDestination(Target.position);
#if UNITY_EDITOR
            Debug.DrawLine(this.transform.position, Target.transform.position, Target == main_target ? Color.green : Color.blue);
#endif
        }
        public override void OnDead()
        {
            Dispatcher.Instance.Dispatch(recycleEventArg);
        }
        public void SetNavStop()
        {
            StartCoroutine(NavStop());
        }
        IEnumerator NavStop()
        {
            for (float t = 0; t < 1; t += Time.deltaTime)
            {
                m_navAgent.speed = Mathf.Lerp(m_navAgent.speed, 0, t);
                yield return null;
            }
        }
        public void SetNavStart()
        {
            StartCoroutine(NavStart());
        }
        IEnumerator NavStart()
        {
            for (float t = 0; t < 1; t += Time.deltaTime)
            {
                m_navAgent.speed = Mathf.Lerp(m_navAgent.speed, m_speed, t);
                yield return null;
            }
        }

        void Awake()
        {
            StateMachine = new StateMachine();

            NormalState = new MinionNormalState(this, StateMachine, detectRange);
            ChaseState = new MinionChaseState(this, StateMachine, attackRange);
            AttackState = new MinionAttackState(this, StateMachine, attackRange);
        }


        public override void Init()
        {
            base.Init();
            m_nowBlood = m_fullBlood;
            recycleEventArg ??= new RecycleEventArg(this.transform);
            StateMachine.Initialize(NormalState);
        }

        void Update()
        {
            StateMachine.CurrentState.Update();
        }
        /// <summary>
        /// 搜尋範圍內的敵人
        /// </summary>
        /// <param name="range">單位範圍</param>
        /// <returns>單位距離</returns>
        public float SearchEnemy(float range)
        {
            Collider[] hits = Physics.OverlapSphere(this.transform.position, range, enemyLayer);
            float closestDistance = float.MaxValue;
            if (hits.Length > 0)
            {
                if (hits.Length == 1 && hits[0].gameObject.tag == "Tower")
                {
                    this.Target = hits[0].transform;
                    closestDistance = Distance(hits[0].transform.position, this.transform.position);
                }
                else
                {
                    Transform closestTarget = null;

                    foreach (var hit in hits)
                    {
                        if (hit.gameObject.tag == "Tower")
                            continue;
                        float distance = Distance(hit.transform.position, this.transform.position);

                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestTarget = hit.transform;
                        }
                    }
                    this.Target = closestTarget;
                }
            }
            return closestDistance;
        }
        float Distance(Vector3 a, Vector3 b)
        {
            return Mathf.Sqrt((b.x - a.x) * (b.x - a.x) + (b.z - a.z) * (b.z - a.z));
        }
#if UNITY_EDITOR
        bool onDrawBoolean = false;
        float onDrawRange;
        
        public void onDrawGizmos(bool onDrawBoolean)
        {
            this.onDrawBoolean = onDrawBoolean;
        }
        public void onDrawGizmos(bool onDrawBoolean, float onDrawRange)
        {
            this.onDrawBoolean = onDrawBoolean;
            this.onDrawRange = onDrawRange;
        }
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (onDrawBoolean)
                Gizmos.DrawWireSphere(this.transform.position, onDrawRange);
        }
        
#region test
    public void Test_Spawn(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        if (ctx.phase == UnityEngine.InputSystem.InputActionPhase.Started)
        {
            if (ctx.control.displayName == "V")
            {
                GameManager.Instance.Spawn("Bullet", this.transform.position + this.transform.forward * 1, this.transform.rotation);
            }
        }
    }
        #endregion
#endif
    }
}