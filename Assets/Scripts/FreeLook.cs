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
    public float moveSpeed = 0.05f, rotateSpeed = 0.1f, zoomSpeed = 1;
    [SerializeField]
    Space rotateSpace = Space.Self;


    void Start()
    {
        playerInput ??= this.GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Free_Move");
    }
    void Update()
    {
        if (allowMove)
            this.transform.Translate(MoveValue(moveAction));

    }
    Vector3 MoveValue(InputAction action)
    {
        Vector3 moveValue = action.ReadValue<Vector3>();
        return moveValue * moveSpeed;
    }
    public void TriggerMove(InputAction.CallbackContext ctx)
    {
        switch (ctx.phase)
        {
            case InputActionPhase.Started:
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
            case InputActionPhase.Started:
                allowRot = true;
                break;
            case InputActionPhase.Canceled:
                allowRot = false;
                break;
        }
    }
    public void Move(InputAction.CallbackContext ctx)
    {
        if (!allowMove)
            return;
        this.transform.Translate(MoveValue(ctx.action));
    }
    public void Rotate(InputAction.CallbackContext ctx)
    {
        if (!allowRot)
           return;
        Vector2 delta = ctx.ReadValue<Vector2>();
        this.transform.Rotate(-delta.y * rotateSpeed, delta.x * rotateSpeed, 0, rotateSpace);
        SetRotationZtoZero();
    }
    public void Grab(InputAction.CallbackContext ctx)
    {
        if (!allowMove)
            return;
        this.transform.Translate(ctx.action.ReadValue<Vector2>() * moveSpeed * -0.1f);
    }
    public void Zoom(InputAction.CallbackContext ctx)
    {
        if (!allowMove)
            return;
        switch (ctx.phase)
        {
            case InputActionPhase.Performed:
                this.transform.Translate(Vector3.forward * ctx.action.ReadValue<float>() * zoomSpeed);
                break;
        }
    }
    void SetRotationZtoZero()
    {
        Vector3 currentEulerAngles = this.transform.localEulerAngles;
        currentEulerAngles.z = 0f;
        this.transform.localEulerAngles = currentEulerAngles;
    }
}
