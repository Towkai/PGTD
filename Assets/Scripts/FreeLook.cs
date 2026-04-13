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
    bool allowMove = false, allowRot = false;
    public float moveSpeed = 0.1f, rotateSpeed = 0.2f, grabSpeed = 0.01f;
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
    public void TriggerMove(InputAction.CallbackContext ctx)
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
    public void TriggerRot(InputAction.CallbackContext ctx)
    {
        switch (ctx.phase)
        {
            case InputActionPhase.Performed:
                allowRot = true;
                break;
            case InputActionPhase.Canceled:
                allowRot = false;
                break;
        }
    }
    public void Move(InputAction.CallbackContext ctx)
    {
        Debug.Log(ctx);
    }
    public void Rotate(InputAction.CallbackContext ctx)
    {
        if (allowRot)
        {
            Vector2 delta = ctx.ReadValue<Vector2>();
            this.transform.Rotate(-delta.y * rotateSpeed, delta.x * rotateSpeed, 0, rotateSpace);
            SetRotationZtoZero(this.transform);
        }
    }
    public void Grab(InputAction.CallbackContext ctx)
    {
        if (allowMove)
        {
            Vector2 delta = ctx.ReadValue<Vector2>();
            this.transform.Translate(delta.x * moveSpeed * -0.1f, delta.y * moveSpeed * -0.1f, 0);
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
