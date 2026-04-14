public abstract class State
{
    public string id;
    protected StateMachine stateMachine;

    public State(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { /*UnityEngine.Debug.Log($"StateEnter: {id}");*/ }
    public virtual void Exit() { /*UnityEngine.Debug.Log($"StateExit: {id}");*/ }
    public virtual void Update() { /*UnityEngine.Debug.Log($"StateUpdate: {id}");*/ }
    public virtual void FixedUpdate() { }
}
public class StateMachine
{
    public State CurrentState { get; private set; }

    /// <summary>
    /// 進入初始狀態
    /// </summary>
    /// <param name="startState"></param>
    public void Initialize(State startState)
    {
        CurrentState = startState;
        CurrentState.Enter();
    }
    
    /// <summary>
    /// 切換狀態
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(State newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}