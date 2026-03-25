using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class NavAI : MonoBehaviour
{
    public Transform target;
    private Coroutine StartChasingTargetCoroutine = null;
    private NavMeshAgent navAgent;
    private float speed = 1;
    WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f); //不要每幀執行
    #if ODIN_INSPECTOR && UNITY_EDITOR

    [Sirenix.OdinInspector.Button("StartChasingTarget")]
    private void StartChasingTargetButton()
    {
        if (target != null && StartChasingTargetCoroutine == null)
            StartCoroutine(StartChasingTarget());
    }
    #endif
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (waitForSeconds == null)
            waitForSeconds = new WaitForSeconds(0.1f);
        if (navAgent == null)
            navAgent = this.GetComponent<NavMeshAgent>();
        if (target != null && StartChasingTargetCoroutine == null)
            StartChasingTargetCoroutine = StartCoroutine(StartChasingTarget());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator StartChasingTarget()
    {
        navAgent.speed = this.speed;
        while (target != null)
        {
            navAgent.SetDestination(target.position);
            yield return waitForSeconds;
        }
        navAgent.speed = 0;
        StartChasingTargetCoroutine = null;
    }
}
