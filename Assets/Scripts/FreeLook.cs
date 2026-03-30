using UnityEngine;
using UnityEngine.InputSystem;

public class FreeLook : MonoBehaviour
{
    // InputAction triggerAction;
    InputAction moveAction; //wsad
    InputAction transAction; //qe
    public float moveSpeed = 1;
    public Vector2 rotateSpeed = Vector2.one;
    [SerializeField]
    Space rotateSpace;
    // InputAction zoomAction;

    private void Start()
    {
        // triggerAction = InputSystem.actions.FindAction("Camera_Trigger");
        moveAction = InputSystem.actions.FindAction("Camera_Move");
        transAction = InputSystem.actions.FindAction("Camera_Translate");
    }

    void Update()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        float translation = transAction.ReadValue<float>();
        if (Mouse.current.rightButton.isPressed || Mouse.current.scroll.value != Vector2.zero)
        {   
            this.transform.Translate(moveValue.x * moveSpeed, translation * moveSpeed, moveValue.y * moveSpeed);
            this.transform.Rotate(-Mouse.current.delta.y.value * rotateSpeed.x, Mouse.current.delta.x.value * rotateSpeed.y, 0, rotateSpace);
            SetRotationZtoZero(this.transform);
        }
    }

    void SetRotationZtoZero(Transform transform)
    {
        Vector3 currentEulerAngles = transform.localEulerAngles;
        currentEulerAngles.z = 0f;
        transform.localEulerAngles = currentEulerAngles;
    }
}
