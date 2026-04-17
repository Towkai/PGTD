using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ObjectPool;
using EventDispatcher;
using Interfaces;
using Unity.Netcode;


public class GameManager : NetworkBehaviour
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
                new Vector3(Random.Range(-14, -10), 0, Random.Range(-4, 4)),
                Quaternion.Euler(0, 90, 0)
                );
                break;
            case "2":
                ObjectPool.SpawnManager.Instance.Spawn(
                ConstString.PooledObject.S_Blue_Minion,
                new Vector3(Random.Range(10, 14), 0, Random.Range(-4, 4)),
                Quaternion.Euler(0, 270, 0)
                );
                break;  
        }
    }
#endregion
    [ServerRpc]
    public void Spawn_Red_Minion_ServerRpc()
    {
        ObjectPool.SpawnManager.Instance.Spawn(
            ConstString.PooledObject.S_Red_Minion,
            new Vector3(Random.Range(-14, -10), 0, Random.Range(-4, 4)),
            Quaternion.Euler(0, 90, 0)
        );
    }
    [ServerRpc]
    public void Spawn_Red_Minion2_ServerRpc()
    {
        ObjectPool.SpawnManager.Instance.Spawn(
            ConstString.PooledObject.S_Red_Minion2,
            new Vector3(Random.Range(-14, -10), 0, Random.Range(-4, 4)),
            Quaternion.Euler(0, 90, 0)
        );
    }
    [ServerRpc]
    public void Spawn_Blue_Minion_ServerRpc()
    {
        ObjectPool.SpawnManager.Instance.Spawn(
            ConstString.PooledObject.S_Blue_Minion,
            new Vector3(Random.Range(10, 14), 0, Random.Range(-4, 4)),
            Quaternion.Euler(0, 90, 0)
        );
    }
    [ServerRpc]
    public void Spawn_Blue_Minion2_ServerRpc()
    {
        ObjectPool.SpawnManager.Instance.Spawn(
            ConstString.PooledObject.S_Blue_Minion2,
            new Vector3(Random.Range(10, 14), 0, Random.Range(-4, 4)),
            Quaternion.Euler(0, 90, 0)
        );
    }

#region test
    public void Test_Spawn(InputAction.CallbackContext ctx)
    {
        Debug.Log(ctx.control.displayName);
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