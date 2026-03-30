using System.Collections;
using UnityEngine;

public class RendomPos : MonoBehaviour
{
    public Vector2 min, max;
    public float coolingSec = 2;
    [SerializeField]
    private Vector3 pos;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SetRendomPos());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SetRendomPos()
    {
        while (true)
        {
            pos = Vector3.right * Random.Range(min.x, max.x) + Vector3.up * this.transform.position.y + Vector3.forward * Random.Range(min.y, max.y);
            this.transform.position = pos;
            yield return new WaitForSeconds(coolingSec);
        }
    }
}
