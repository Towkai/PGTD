using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public float speed = 10f;

    void Update()
    {
        if (!IsServer) return;

        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;

        if (other.TryGetComponent<Character.CharacterBase>(out var target))
        {
            target.GetInjured(1);
        }

        GetComponent<NetworkObject>().Despawn();
    }
}