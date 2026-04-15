using UnityEngine;
using UnityEngine.InputSystem;

namespace EventDispatcher
{
    public class InputController : MonoBehaviour
    {
        [SerializeField] 
        private string _logMessage;

        private void Update()
        {
            if (Mouse.current.leftButton.value > 0)
            {
                Dispatcher.Instance.Dispatch(new ConsoleLogEvent(_logMessage));
            }
        }
    }
}