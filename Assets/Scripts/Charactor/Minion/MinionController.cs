using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

namespace Character
{
    public class MinionController : CharacterBase
    {
        public StateMachine StateMachine { get; private set; }
        public MinionNormalState NormalState { get; private set; }
        public MinionChaseState ChaseState { get; private set; }
        public MinionAttackState AttackState { get; private set; }

        [SerializeField] private NavMeshAgent m_navAgent;
        [SerializeField] private float m_speed;

        public float detectRange = 3.5f;
        public float attackRange = 2f;
        public float attackDelay = 1f;
        public LayerMask enemyLayer;

        private Transform main_target;

        // ✅ 同步 Target
        private NetworkVariable<ulong> targetId = new NetworkVariable<ulong>();

        public Transform Target
        {
            get
            {
                if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(targetId.Value, out var obj))
                    return obj.transform;
                return main_target;
            }
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                Init();
            }

            // ❗ Client 不跑 NavMesh
            if (!IsServer && m_navAgent != null)
                m_navAgent.enabled = false;
        }

        public override void Init()
        {
            base.Init();

            StateMachine = new StateMachine();

            NormalState = new MinionNormalState(this, StateMachine, detectRange);
            ChaseState = new MinionChaseState(this, StateMachine, attackRange);
            AttackState = new MinionAttackState(this, StateMachine, attackRange);

            main_target = GameObject.Find("Tower")?.transform;

            StateMachine.Initialize(NormalState);
        }

        void Update()
        {
            if (!IsServer) return;

            StateMachine?.CurrentState.Update();
        }

        public void SetNavDestination()
        {
            if (!IsServer) return;

            if (Target != null)
                m_navAgent.SetDestination(Target.position);
        }

        public void SetTarget(Transform target)
        {
            if (!IsServer) return;

            if (target.TryGetComponent<NetworkObject>(out var netObj))
                targetId.Value = netObj.NetworkObjectId;
        }

        public float SearchEnemy(float range)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, range, enemyLayer);

            float closestDistance = float.MaxValue;
            Transform closestTarget = null;

            foreach (var hit in hits)
            {
                if (hit.CompareTag("Tower")) continue;

                float distance = Vector3.Distance(hit.transform.position, transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = hit.transform;
                }
            }

            if (closestTarget != null)
                SetTarget(closestTarget);

            return closestDistance;
        }

        public void Attack()
        {
            if (!IsServer) return;

            // TODO: 傷害計算

            PlayAttackClientRpc();
        }

        [ClientRpc]
        void PlayAttackClientRpc()
        {
            Debug.Log("Play Attack Animation");
        }
    }
}