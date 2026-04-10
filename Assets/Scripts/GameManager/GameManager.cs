using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ObjectPool;
using EventDispatcher;
using Interfaces;
using Unity.Netcode;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public SpawnManager SpawnManager => SpawnManager.Instance;
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

#region chat
    public void OnChat(string msg)
    {
        switch (msg)
        {
            case "1":
                Spawn(ConstString.S_Red_Minion);
                break;            
        }
    }
#endregion

#region test
    public void Test_Spawn(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            if (ctx.control.displayName == "Z")
            {
                Spawn(ConstString.S_Red_Minion);
            }
            else if (ctx.control.displayName == "X")
            {
                Spawn(ConstString.S_Blue_Minion);
            }
            else if (ctx.control.displayName == "C")
            {
                Dispatcher.Instance.Dispatch(new RecycleEvent());
            }
        }
    }
#endregion
}