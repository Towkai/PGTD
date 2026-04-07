using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ObjectPool;
using EventDispatcher;
using Interfaces;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField]
    PlayerInput testInput;
    List<InputAction> spawnAction = new List<InputAction>();

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        testInput ??= this.GetComponent<PlayerInput>();
        spawnAction.Add(testInput.actions.FindAction("Spawn_S.Red_Minion"));
        spawnAction.Add(testInput.actions.FindAction("Spawn_S.Blue_Minion"));
    }
    public bool Spawn(string key, Vector3? pos = null, Quaternion? rot = null)
    {
        var spawn = SpawnManager.Instance.Spawn(key, pos, rot);
        return spawn.activeInHierarchy;
    }
#region test
    public void Test_Spawn(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            if (ctx.control.displayName == "Z")
            {
                Spawn("S.Red.Minion");
            }
            else if (ctx.control.displayName == "X")
            {
                Spawn("S.Blue.Minion");
            }
            else if (ctx.control.displayName == "C")
            {
                Dispatcher.Instance.Dispatch(new RecycleEvent());
            }
        }
    }
#endregion
}