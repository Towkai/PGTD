using Unity.Netcode;
using UnityEngine;

public class NetworkController : NetworkBehaviour
{
    public static NetworkController Instance;

    [Header("Prefabs")]
    [SerializeField] GameObject minion;

    [Header("Score")]
    public NetworkVariable<int> scoreA = new NetworkVariable<int>();
    public NetworkVariable<int> scoreB = new NetworkVariable<int>();

    void Awake()
    {
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        // 監聽分數變化（Client也會收到）
        scoreA.OnValueChanged += (oldVal, newVal) =>
        {
            Debug.Log($"Score A: {newVal}");
        };

        scoreB.OnValueChanged += (oldVal, newVal) =>
        {
            Debug.Log($"Score B: {newVal}");
        };
    }

    // =========================
    // 🎯 給 Host 呼叫（直播事件）
    // =========================
    public void OnGiftFromTeamA()
    {
        if (!IsServer) return;

        scoreA.Value += 10;

        SpawnMonsterClientRpc();
    }

    public void OnGiftFromTeamB()
    {
        if (!IsServer) return;

        scoreB.Value += 10;

        SpawnMonsterClientRpc();
    }

    // =========================
    // 🎯 同步生成怪物
    // =========================
    [ClientRpc]
    void SpawnMonsterClientRpc()
    {
        Vector3 pos = Random.insideUnitSphere * 5f;
        Instantiate(minion, pos, Quaternion.identity);
    }
}