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
    public bool Spawn(string key, Vector3 pos, Quaternion rot)
    {
        var spawn = SpawnManager.Instance.Spawn(key, pos, rot);
        return spawn != null;
    }

#region chat
    public void OnChat(string msg)
    {
        switch (msg)
        {
            case "1":
                SpawnManager.Instance.Spawn(
                ConstString.PooledObject.S_Red_Minion,
                new Vector3(Random.Range(-14, -10), 0, Random.Range(-4, 4)),
                Quaternion.Euler(0, 90, 0)
                );
                break;
            case "2":
                SpawnManager.Instance.Spawn(
                ConstString.PooledObject.S_Blue_Minion,
                new Vector3(Random.Range(10, 14), 0, Random.Range(-4, 4)),
                Quaternion.Euler(0, 270, 0)
                );
                break;  
        }
    }
#endregion

    public void Spawn_Red_Minion()
    {
        SpawnManager.Instance.Spawn(
            ConstString.PooledObject.S_Red_Minion,
            new Vector3(Random.Range(-14, -10), 0, Random.Range(-4, 4)),
            Quaternion.Euler(0, 90, 0)
        );
    }
    public void Spawn_Red_Minion2()
    {
        SpawnManager.Instance.Spawn(
            ConstString.PooledObject.S_Red_Minion2,
            new Vector3(Random.Range(-14, -10), 0, Random.Range(-4, 4)),
            Quaternion.Euler(0, 90, 0)
        );
    }
    public void Spawn_Blue_Minion()
    {
        SpawnManager.Instance.Spawn(
            ConstString.PooledObject.S_Blue_Minion,
            new Vector3(Random.Range(10, 14), 0, Random.Range(-4, 4)),
            Quaternion.Euler(0, 90, 0)
        );
    }
    public void Spawn_Blue_Minion2()
    {
        SpawnManager.Instance.Spawn(
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
                    Spawn_Red_Minion();
                }
                else if (ctx.control.displayName == Key.X.ToString())
                {
                    Spawn_Red_Minion2();
                }
                else if (ctx.control.displayName == "/")
                {
                    Debug.Log("Spawn_Blue_Minion");
                    Spawn_Blue_Minion();
                }
                else if (ctx.control.displayName == ".")
                {
                    Debug.Log("Spawn_Blue_Minion2");
                    Spawn_Blue_Minion2();
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