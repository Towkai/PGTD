using EventDispatcher;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public float speed = 10f;

    private bool isDespawned;

    void Update()
    {
        if (!IsServer) return;

        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer || isDespawned) return;

        isDespawned = true;

        if (other.TryGetComponent<Character.CharacterBase>(out var target))
        {
            target.GetInjured(1);
        }

        Dispatcher.Instance.Dispatch(new Interfaces.RecycleEventArg(this.transform));
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        isDespawned = false;
    }
}