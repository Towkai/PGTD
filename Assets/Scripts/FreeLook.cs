using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FreeLook : MonoBehaviour
{
    [SerializeField]
    PlayerInput playerInput;
    InputAction moveAction; //wsadqe
    [SerializeField]
    bool allowMove = false;
    public float moveSpeed = 0.1f;
    public float rotateSpeed = 0.2f;
    [SerializeField]
    Space rotateSpace;

    void Start()
    {
        playerInput ??= this.GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Free_Move");
    }
    void Update()
    {
        if (allowMove)
            Move(moveSpeed);
    }
    void Move(float speed)
    {
        Vector3 moveValue = moveAction.ReadValue<Vector3>();
        this.transform.Translate(moveValue.x * speed, moveValue.y * speed, moveValue.z * speed);        
    }
    public void Move(InputAction.CallbackContext ctx)
    {
        switch (ctx.phase)
        {
            case InputActionPhase.Performed:
                allowMove = true;
                break;
            case InputActionPhase.Canceled:
                allowMove = false;
                break;
        }
    }
    public void Rotate(InputAction.CallbackContext ctx)
    {
        if (allowMove && ctx.performed)
        {
            Vector2 delta = ctx.ReadValue<Vector2>();
            this.transform.Rotate(-delta.y * rotateSpeed, delta.x * rotateSpeed, 0, rotateSpace);
            SetRotationZtoZero(this.transform);
        }
    }
    public void Zoom(InputAction.CallbackContext ctx)
    {
        switch (ctx.phase)
        {
            case InputActionPhase.Started:
                moveSpeed *= 2;
                break;
            case InputActionPhase.Performed:
                moveSpeed /= 2;
                break;
        }
    }
    void SetRotationZtoZero(Transform transform)
    {
        Vector3 currentEulerAngles = transform.localEulerAngles;
        currentEulerAngles.z = 0f;
        transform.localEulerAngles = currentEulerAngles;
    }
}
