using Unity.VisualScripting;
using UnityEngine;

namespace Charactor
{
    public class MinionNormalState : State //追主堡
    {
        private MinionController minion;

        private float detectRange; //搜敵範圍
        private LayerMask enemyLayer;
        public MinionNormalState(MinionController minion, StateMachine stateMachine, float detectRange): base(stateMachine)
        {
            base.id = "Normal";
            this.minion = minion;
            this.detectRange = detectRange;
        }
        public override void Enter()
        {
            minion.Target = null;
            minion.SetNavStart();
#if UNITY_EDITOR
            minion.onDrawGizmos(true, detectRange);
#endif
        }
        public override void Update()
        {
            if (minion.SearchEnemy(detectRange) < detectRange)
                stateMachine.ChangeState(minion.ChaseState);
            minion.SetNavDestination();
        }
        public override void Exit()
        {      
#if UNITY_EDITOR
            minion.onDrawGizmos(false);
#endif
        }
    }
    public class MinionChaseState : State //追小怪
    {
        private MinionController minion;
        private float attackRange; //攻擊範圍

        public MinionChaseState(MinionController minion, StateMachine stateMachine, float detectRange): base(stateMachine)
        {
            base.id = "Chase";
            this.minion = minion;
            this.attackRange = detectRange;
        }
        public override void Enter()
        {
#if UNITY_EDITOR
            minion.onDrawGizmos(true, attackRange);
#endif
            minion.SetNavStart();
        }
        public override void Update()
        {
            if (minion.SearchEnemy(attackRange) < attackRange)
                stateMachine.ChangeState(minion.AttackState);
            minion.SetNavDestination();
        }
        public override void Exit()
        {      
#if UNITY_EDITOR
            minion.onDrawGizmos(false);
#endif
        }
    }
    public class MinionAttackState : State //攻擊
    {
        private MinionController minion;

        public MinionAttackState(MinionController minion, StateMachine stateMachine): base(stateMachine)
        {
            base.id = "Attack";
            this.minion = minion;
        }
        public override void Enter()
        {
            minion.SetNavStop();
        }
        public override void Update()
        {
            if (minion.Target.tag == "Tower")
                stateMachine.ChangeState(minion.NormalState);
        }
    }
}