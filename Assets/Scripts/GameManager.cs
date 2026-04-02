using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ObjectPool;
using EventDispatcher;
using Interfaces;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    PlayerInput testInput;
    List<InputAction> spawnAction = new List<InputAction>();

    private void Start()
    {
        testInput ??= this.GetComponent<PlayerInput>();
        spawnAction.Add(testInput.actions.FindAction("Spawn_S.Red_Minion"));
        spawnAction.Add(testInput.actions.FindAction("Spawn_S.Blue_Minion"));
    }
#region test
    public void Test_Spawn(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            if (ctx.control.displayName == "Z")
            {
                //TODO: 生成紅方小兵
                SpawnManager.Instance.Spawn(
                    "S.Red.Minion"
                );
            }
            else if (ctx.control.displayName == "X")
            {
                //TODO: 生成藍方小兵    
                SpawnManager.Instance.Spawn(
                    "S.Blue.Minion"
                );
            }
            else if (ctx.control.displayName == "C")
            {
                //TODO: 回收所有小兵
                Dispatcher.Instance.Dispatch(new EmptyEvent());
            }
        }
    }
#endregion
}