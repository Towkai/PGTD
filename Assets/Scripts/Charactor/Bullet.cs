using EventDispatcher;
using Interfaces;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Bullet : ObjectPool.PooledObject
{
    public float speed = 10f;
    private bool isDespawned;
    public LayerMask companionLayer;
    [SerializeField] private TrailRenderer m_trailRenderer = null;
    private RecycleEventArg recycleEventArg;

    public bool IsEnemyLayerMask(GameObject obj)
    {
        if (obj.layer == 1)
            return false;
        return (companionLayer.value | (1 << obj.layer)) > companionLayer;
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        recycleEventArg = new RecycleEventArg(this.transform);
    }
    public void Init() //由PooledObject.OnSpawn呼叫
    {
        isDespawned = false;
    }
    void Update()
    {
        if (!IsServer) return;

        transform.position += transform.forward * speed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer || isDespawned || !IsEnemyLayerMask(other.gameObject)) return;
        isDespawned = true;

        if (other.TryGetComponent<Character.CharacterBase>(out var target))
        {
            target.GetInjured(1);
        }

        Dispatcher.Instance.Dispatch(recycleEventArg);
    }
}