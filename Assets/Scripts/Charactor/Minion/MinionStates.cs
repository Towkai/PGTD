using EventDispatcher;
using Interfaces;
using Unity.VisualScripting;
using UnityEngine;

namespace Character
{
    public class MinionBaseState : State //追主堡
    {
        protected MinionController minion;

        public MinionBaseState(MinionController minion, StateMachine stateMachine, string stateId): base(stateMachine)
        {
            this.minion = minion;
            base.id = stateId;
            minion.StateId = stateId;
        }
        public override void Enter() { base.Enter(); minion.StateId = id; }
        public override void Exit() { base.Exit(); }
        public override void Update() {  base.Update(); }
        public override void FixedUpdate() { base.FixedUpdate(); }
    }

    public class MinionNormalState : MinionBaseState //追主堡
    {

        private float detectRange; //搜敵範圍
        public MinionNormalState(MinionController minion, StateMachine stateMachine, float detectRange): base(minion, stateMachine, "Normal")
        {
            this.detectRange = detectRange;
        }
        public override void Enter()
        {
            base.Enter();
            minion.StartNavDestination();
#if UNITY_EDITOR
            minion.onDrawGizmos(true, detectRange);
#endif
        }
        public override void Update()
        {
            base.Update();
            if (minion.SearchEnemy(detectRange) < detectRange)
                stateMachine.ChangeState(minion.ChaseState);
        }
        public override void Exit()
        {      
            base.Exit();
#if UNITY_EDITOR
            minion.onDrawGizmos(false);
#endif
            minion.StopNavDestination();
        }
    }
    public class MinionChaseState : MinionBaseState //追小怪
    {
        private float attackRange; //攻擊範圍

        public MinionChaseState(MinionController minion, StateMachine stateMachine, float detectRange): base(minion, stateMachine, "Chase")
        {
            this.attackRange = detectRange;
        }
        public override void Enter()
        {
            base.Enter();
#if UNITY_EDITOR
            minion.onDrawGizmos(true, attackRange);
#endif
            minion.StartNavDestination();
        }
        public override void Update()
        {
            base.Update();
            if (minion.SearchEnemy(attackRange) < attackRange)
                stateMachine.ChangeState(minion.AttackState);
        }
        public override void Exit()
        {      
            base.Exit();
#if UNITY_EDITOR
            minion.onDrawGizmos(false);
#endif
            minion.StopNavDestination();
        }
    }
    public class MinionAttackState : MinionBaseState //攻擊
    {
        private float timer;
        private float attackRange; //攻擊範圍

        public MinionAttackState(MinionController minion, StateMachine stateMachine, float detectRange): base(minion, stateMachine, "Attack")
        {
            this.attackRange = detectRange;
        }
        public override void Enter()
        {
            base.Enter();
            minion.StopNavDestination();
        }
        public override void Update()
        {
            base.Update();
            if (minion.SearchEnemy(attackRange) > attackRange)
                stateMachine.ChangeState(minion.NormalState);
#region 攻擊週期
            if (timer == 0)
                minion.AttackClientRpc();
            else if (timer > minion.attackDelay)
            {
                minion.AttackClientRpc();
                timer = 0;
            }
            timer += Time.deltaTime;
#endregion
        }
        public override void Exit()
        {      
            base.Exit();
        }
    }
    public class MinionDeadState : MinionBaseState //死亡
    {
        RecycleEventArg recycleEventArg;
        public MinionDeadState(MinionController minion, StateMachine stateMachine): base(minion, stateMachine, "Dead")
        {
            recycleEventArg = new RecycleEventArg(minion.transform/*, minion.Init*/); //改由PooledObject.OnRecycle呼叫初始化，在Inspector設定
        }
        public override void Enter()
        {
            base.Enter();
            minion.Init();
            Dispatcher.Instance.Dispatch(recycleEventArg);
        }
        public override void Update()
        {
            base.Update();
        }
        public override void Exit()
        {      
            base.Exit();
        }
    }
}