using System.Collections;
using UnityEngine;

public class Lookat : MonoBehaviour
{
    // [SerializeField]
    // float waitSeconds = 0.1f;
    [SerializeField]
    Transform target = null;
    void Start()
    {
        if (target == null)
            target = Camera.main.transform;
    }
    void Update()
    {
        if (target == null)
            return;
        this.transform.forward = target.forward;            
            // this.transform.forward = this.transform.position - target.position;
            // SetRotationZtoZero(this.transform);
    }
    
    void SetRotationZtoZero(Transform transform)
    {
        Vector3 currentEulerAngles = transform.localEulerAngles;
        // currentEulerAngles.z = 0f;
        // transform.localEulerAngles = currentEulerAngles;
        transform.Rotate(transform.forward, -currentEulerAngles.z);
    }
}
