using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ObjectPool;
using EventDispatcher;
using Interfaces;
using Unity.Netcode;

public enum ESide { Red, Blue }
public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;
    [SerializeField]
    PlayerInput testInput;
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        testInput ??= this.GetComponent<PlayerInput>();
    }
    public bool Spawn(string key, Vector3 pos, Quaternion rot)
    {
        var spawn = ObjectPool.SpawnManager.Instance.Spawn(key, pos, rot);
        return spawn != null;
    }

#region chat
    public void OnChat(string msg)
    {
        switch (msg)
        {
            case "1":
                ObjectPool.SpawnManager.Instance.Spawn(
                ConstString.PooledObject.S_Red_Minion,
                ESide.Red
                );
                break;
            case "2":
                ObjectPool.SpawnManager.Instance.Spawn(
                ConstString.PooledObject.S_Blue_Minion,
                ESide.Blue
                );
                break;  
        }
    }
#endregion
    public void OnSpawnButtonClick(string name)
    {
        switch (name)
        {
            case ConstString.PooledObject.S_Red_Minion:
                Spawn_Red_Minion_ServerRpc(name);
                break;
            case ConstString.PooledObject.S_Red_Minion2:
                Spawn_Red_Minion2_ServerRpc();
                break;
            case ConstString.PooledObject.S_Blue_Minion:
                Spawn_Blue_Minion_ServerRpc();
                break;
            case ConstString.PooledObject.S_Blue_Minion2:
                Spawn_Blue_Minion2_ServerRpc();
                break;
        }
    }
    [Rpc(SendTo.Server)]
    public void Spawn_Red_Minion_ServerRpc(string name)
    {
        Debug.Log("Spawn_Red_Minion_ServerRpc");
        ObjectPool.SpawnManager.Instance.Spawn(
            name,
            ESide.Red
        );
    }
    [Rpc(SendTo.Server)]
    public void Spawn_Red_Minion_ServerRpc()
    {
        Debug.Log("Spawn_Red_Minion_ServerRpc");
        ObjectPool.SpawnManager.Instance.Spawn(
            ConstString.PooledObject.S_Red_Minion,
            ESide.Red
        );
    }
    [Rpc(SendTo.Server)]
    public void Spawn_Red_Minion2_ServerRpc()
    {
        Debug.Log("Spawn_Red_Minion2_ServerRpc");
        ObjectPool.SpawnManager.Instance.Spawn(
            ConstString.PooledObject.S_Red_Minion2,
            ESide.Red
        );
    }
    [Rpc(SendTo.Server)]
    public void Spawn_Blue_Minion_ServerRpc()
    {
        Debug.Log("Spawn_Blue_Minion_ServerRpc");
        ObjectPool.SpawnManager.Instance.Spawn(
            ConstString.PooledObject.S_Blue_Minion,
            ESide.Blue
        );
    }
    [Rpc(SendTo.Server)]
    public void Spawn_Blue_Minion2_ServerRpc()
    {
        Debug.Log("Spawn_Blue_Minion2_ServerRpc");
        ObjectPool.SpawnManager.Instance.Spawn(
            ConstString.PooledObject.S_Blue_Minion2,
            ESide.Blue
        );
    }

#region test
    public void Test_Spawn(InputAction.CallbackContext ctx)
    {
        switch (ctx.phase)
        {
            case InputActionPhase.Started:
                if (ctx.control.displayName == Key.Z.ToString())
                {
                    Spawn_Red_Minion_ServerRpc();
                }
                else if (ctx.control.displayName == Key.X.ToString())
                {
                    Spawn_Red_Minion2_ServerRpc();
                }
                else if (ctx.control.displayName == "/")
                {
                    Spawn_Blue_Minion_ServerRpc();
                }
                else if (ctx.control.displayName == ".")
                {
                    Spawn_Blue_Minion2_ServerRpc();
                }
                else if (ctx.control.displayName == "C")
                {
                    Dispatcher.Instance.Dispatch(new RecycleEvent());
                }
                break;
        }
    }

#endregion
}