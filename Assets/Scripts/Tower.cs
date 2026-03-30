using UnityEngine;
using UnityEngine.Events;

public class Tower : MonoBehaviour
{
#region Blood
    [SerializeField]
    Transform bloodObject;
    [SerializeField]
    float oriBlood = 1000;
    float nowBlood;
    [SerializeField]
    Vector3 oriScale = new Vector3(2, 4, 1);
    Vector3 nowScale = new Vector3(2, 4, 1);

    void SetBloodView()
    {
        nowScale.x = Mathf.Lerp(0, oriScale.x, nowBlood / oriBlood);
        bloodObject.localScale = nowScale;
    }
#endregion
    public enum Side
    {
        Red, Blue
    }
    public Side side = Side.Red;
    [SerializeField]
    UnityEvent onDeath;
    
    public void init()
    {
        nowBlood = oriBlood;
        oriScale = bloodObject.localScale;
        nowScale = oriScale;
        SetBloodView();
    }
    public void GetHarm(float harm)
    {
        nowBlood -= harm;
        SetBloodView();
        if (nowBlood <= 0)
            onDeath?.Invoke();
    }

    void Start()
    {
        init();
    }
}
