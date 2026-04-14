using System.Collections;
using EventDispatcher;
using Interfaces;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

namespace Character
{
    public class MinionController : CharacterBase
    {
        public string StateId = string.Empty;
        public StateMachine StateMachine { get; private set; }
        public MinionNormalState NormalState { get; private set; }
        public MinionChaseState ChaseState { get; private set; }
        public MinionAttackState AttackState { get; private set; }
        public MinionDeadState DeadState { get; private set; }

        [SerializeField] private NavMeshAgent m_navAgent;
        private Coroutine m_navCoroutine = null;
        public bool hasDestination;
        [SerializeField] private float m_speed;

        public float detectRange = 3.5f;
        public float attackRange = 2f;
        public float attackDelay = 1f;
        public LayerMask enemyLayer;

        [SerializeField] private Transform main_target = null;
        [SerializeField] private Transform m_target = null;


        public Transform Target => m_target == null || !m_target.gameObject.activeInHierarchy ? main_target : m_target;
        public bool IsInLayerMask(GameObject obj)
        {
            return (enemyLayer.value | (1 << obj.layer)) == enemyLayer;
        }
        public override void OnNetworkSpawn()
        {
            // 只有 Server 會跑 NavMesh
            if (m_navAgent != null)
                m_navAgent.enabled = IsServer;
        }

        public override void Init()
        {
            base.Init();
            StateMachine?.Initialize(NormalState);
        }
        void OnEnable()
        {
            if (IsServer)
                Init();
        }
        void Awake()
        {
            StateMachine = new StateMachine();

            AttackState = new MinionAttackState(this, StateMachine, attackRange);
            ChaseState = new MinionChaseState(this, StateMachine, attackRange);
            NormalState = new MinionNormalState(this, StateMachine, detectRange);
            DeadState = new MinionDeadState(this, StateMachine);

            if (main_target == null)
            {
                GameObject[] towers =  GameObject.FindGameObjectsWithTag("Tower");
                foreach (var tower in towers)
                    if (IsInLayerMask(tower))
                        main_target = tower.transform;
            }         
        }
        void Update()
        {
            if (!IsServer) return;

            StateMachine?.CurrentState.Update();
        }
        public void StartNavDestination()
        {
            if (!IsServer) return;
            if (m_navCoroutine != null)
                StopCoroutine(m_navCoroutine);
            m_navCoroutine = StartCoroutine(SetNavDestination());
        }
        public void StopNavDestination()
        {
            if (!IsServer) return;
            hasDestination = false;
        }
        IEnumerator SetNavDestination()
        {
            if (!IsServer) yield break;
            if (Target != null)
            {
                hasDestination = true;
                while (hasDestination)
                {
                    m_navAgent.SetDestination(Target.position);
                    m_navAgent.speed = m_speed;
                    for (float t = 0; t < 0.2f; t += Time.deltaTime) 
                    {
#if UNITY_EDITOR
                        Debug.DrawLine(this.transform.position, Target.transform.position, Target == main_target ? Color.green : Color.blue);
#endif
                        yield return null;
                    }
                }
                float stopTime = 1;
                for (float t = stopTime; t > 0; t -= Time.deltaTime / stopTime)
                {
                    m_navAgent.speed = t;
                    yield return null;
                }
                m_navAgent.speed = 0;
                m_navCoroutine = null;
            }
        }

        public void SetTarget(Transform target)
        {
            if (!IsServer) return;
            this.m_target = target;
        }

        public float SearchEnemy(float range)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, range, enemyLayer);

            float closestDistance = float.MaxValue;
            Transform closestTarget = null;
            float distance;
            if (hits.Length == 0)
                closestTarget = null;
            else if (hits.Length == 1)
            {
                closestTarget = hits[0].transform;
                closestDistance = Vector3.Distance(hits[0].transform.position, transform.position);
            }
            else
            {                    
                foreach (var hit in hits)
                {
                    if (hit.CompareTag("Tower")) continue;

                    distance = Vector3.Distance(hit.transform.position, transform.position);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestTarget = hit.transform;
                    }
                }
            }

            SetTarget(closestTarget);

            return closestDistance;
        }
        [ClientRpc]
        public void AttackClientRpc()
        {
            if (!IsServer) return;
            this.transform.forward = Vector3.ProjectOnPlane(Target.position - this.transform.position, Vector3.up);
            GameManager.Instance.Spawn(ConstString.Bullet, this.transform.position + this.transform.forward * 0.5f, this.transform.rotation);
        }
        public override void OnDead()
        {
            base.OnDead();
            StateMachine.ChangeState(DeadState);
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
#endif
    }
}