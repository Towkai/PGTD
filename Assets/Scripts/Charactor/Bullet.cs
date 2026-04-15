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

    private RecycleEventArg recycleEventArg;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        recycleEventArg = new RecycleEventArg(this.transform, Init);
    }
    void Init()
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
        if (!IsServer || isDespawned) return;
        isDespawned = true;   


        bool isCharacter = other.TryGetComponent<Character.CharacterBase>(out var target);


        if (isCharacter)
        {
            target.GetInjured(1);
        }

        Dispatcher.Instance.Dispatch(recycleEventArg);
    }
}