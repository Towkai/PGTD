using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public enum Side {red, blue}
    // InputAction triggerAction;
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
                    "Minion",
                    Vector3.zero,
                    Quaternion.Euler(0, 0, 0)
                );
            }
            else if (ctx.control.displayName == "X")
            {
                //TODO: 生成藍方小兵    
            }
            else if (ctx.control.displayName == "C")
            {
                //TODO: 回收所有小兵
            }
        }
    }
#endregion
}