using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FreeLook : MonoBehaviour
{
    // InputAction triggerAction;
    [SerializeField]
    PlayerInput playerInput;
    InputAction moveAction; //wsadqe
    public float moveSpeed = 0.1f;
    public float zoomSpeed = 0.2f;
    public Vector2 rotateSpeed = Vector2.one * 0.2f;
    [SerializeField]
    Space rotateSpace;
    // InputAction zoomAction;

    private void Start()
    {
        playerInput ??= this.GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Free_Move");
    }

    void Update()
    {
        if (Mouse.current.rightButton.isPressed)
        {   
            Move(moveSpeed);
            Rotate();
        }
        if (Mouse.current.scroll.value != Vector2.zero)
        {
            Move(zoomSpeed);
        }
    }
    void Move(float speed)
    {
        Vector3 moveValue = moveAction.ReadValue<Vector3>();
        this.transform.Translate(moveValue.x * speed, moveValue.y * speed, moveValue.z * speed);        
    }
    void Rotate()
    {
        this.transform.Rotate(-Mouse.current.delta.y.value * rotateSpeed.x, Mouse.current.delta.x.value * rotateSpeed.y, 0, rotateSpace);
        SetRotationZtoZero(this.transform);
    }
    void SetRotationZtoZero(Transform transform)
    {
        Vector3 currentEulerAngles = transform.localEulerAngles;
        currentEulerAngles.z = 0f;
        transform.localEulerAngles = currentEulerAngles;
    }
}
