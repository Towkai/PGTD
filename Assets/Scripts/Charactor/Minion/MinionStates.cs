using Unity.VisualScripting;
using UnityEngine;

namespace Character
{
    public class MinionBaseState : State //追主堡
    {
        protected MinionController minion;

        public MinionBaseState(MinionController minion, StateMachine stateMachine): base(stateMachine)
        {
            this.minion = minion;
        }
        public override void Enter() { base.Enter(); }
        public override void Exit() { base.Exit(); }
        public override void Update() {  base.Update(); }
        public override void FixedUpdate() { base.FixedUpdate(); }
    }

    public class MinionNormalState : MinionBaseState //追主堡
    {

        private float detectRange; //搜敵範圍
        public MinionNormalState(MinionController minion, StateMachine stateMachine, float detectRange): base(minion, stateMachine)
        {
            base.id = "Normal";
            this.detectRange = detectRange;
        }
        public override void Enter()
        {
            base.Enter();
            minion.Target = null;
            minion.SetNavStart();
#if UNITY_EDITOR
            minion.onDrawGizmos(true, detectRange);
#endif
        }
        public override void Update()
        {
            base.Update();
            if (minion.SearchEnemy(detectRange) < detectRange)
                stateMachine.ChangeState(minion.ChaseState);
            minion.SetNavDestination();
        }
        public override void Exit()
        {      
            base.Exit();
#if UNITY_EDITOR
            minion.onDrawGizmos(false);
#endif
        }
    }
    public class MinionChaseState : MinionBaseState //追小怪
    {
        private float attackRange; //攻擊範圍

        public MinionChaseState(MinionController minion, StateMachine stateMachine, float detectRange): base(minion, stateMachine)
        {
            base.id = "Chase";
            this.attackRange = detectRange;
        }
        public override void Enter()
        {
            base.Enter();
#if UNITY_EDITOR
            minion.onDrawGizmos(true, attackRange);
#endif
            minion.SetNavStart();
        }
        public override void Update()
        {
            base.Update();
            if (minion.SearchEnemy(attackRange) < attackRange)
                stateMachine.ChangeState(minion.AttackState);
            minion.SetNavDestination();
        }
        public override void Exit()
        {      
            base.Exit();
#if UNITY_EDITOR
            minion.onDrawGizmos(false);
#endif
        }
    }
    public class MinionAttackState : MinionBaseState //攻擊
    {
        private float timer;
        private float attackRange; //攻擊範圍

        public MinionAttackState(MinionController minion, StateMachine stateMachine, float detectRange): base(minion, stateMachine)
        {
            base.id = "Attack";
            this.attackRange = detectRange;
        }
        public override void Enter()
        {
            base.Enter();
            minion.SetNavStop();
        }
        public override void Update()
        {
            base.Update();
            if (minion.SearchEnemy(attackRange) > attackRange)
                stateMachine.ChangeState(minion.NormalState);
#region 攻擊週期
            timer += Time.deltaTime;
            if (timer > minion.attackDelay)
            {
                Attack();
                timer = 0;
            }
#endregion
        }
        void Attack()
        {
            minion.transform.forward = Vector3.ProjectOnPlane(minion.Target.position - minion.transform.position, Vector3.up);
            GameManager.Instance.Spawn(ConstString.Bullet, minion.transform.position + minion.transform.forward * 0.5f, minion.transform.rotation);
        }
        public override void Exit()
        {      
            base.Exit();
        }
    }
}