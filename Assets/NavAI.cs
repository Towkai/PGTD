using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NavAI : MonoBehaviour
{
    private NavMeshAgent navAgent;
    public Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (navAgent == null)
            navAgent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator StartChasingTarget(Transform target)
    {
        while (target != null)
        {
            navAgent.SetDestination(target.position);
            yield return null;
        }
    }
}
