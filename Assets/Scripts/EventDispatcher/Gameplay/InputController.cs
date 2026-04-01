using UnityEngine;

namespace EventDispatcher
{
    public class InputController : MonoBehaviour
    {
        [SerializeField] 
        private string _logMessage;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Dispatcher.Instance.Dispatch(new ConsoleLogEvent(_logMessage));
            }
        }
    }
}