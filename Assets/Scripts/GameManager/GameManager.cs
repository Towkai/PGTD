using UnityEngine;
using ObjectPool;
using Unity.Netcode;
using Unity.Android.Gradle.Manifest;

public enum ESide { none, Red, Blue }
public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;
    private SpawnManager m_spawnManager = null;
    public SpawnManager SpawnManager => m_spawnManager ??= FindFirstObjectByType<SpawnManager>();
    public ESide MySide => IsHost ? ESide.Red : ESide.Blue;
    void Awake () 
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy (gameObject);
                
        DontDestroyOnLoad (gameObject);
    }
    public bool Spawn(string key, Vector3 pos, Quaternion rot)
    {
        var spawn = SpawnManager.Spawn(key, pos, rot);
        return spawn != null;
    }

#region chat
    public void OnChat(string msg)
    {
        switch (msg)
        {
            case "1":
                SpawnManager.Spawn(
                Data.ConstString.PooledObject.S_Red_Minion,
                ESide.Red
                );
                break;
            case "2":
                SpawnManager.Spawn(
                Data.ConstString.PooledObject.S_Blue_Minion,
                ESide.Blue
                );
                break;  
        }
    }
#endregion
}