using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventDispatcher
{
    public class ConsoleLogger : MonoBehaviour
    {
        private void OnEnable()
        {
            Dispatcher.Instance.Subscribe<ConsoleLogEvent>(DisplayLogMessage);
        }

        private void OnDisable()
        {
            Dispatcher.Instance.Unsubscribe<ConsoleLogEvent>(DisplayLogMessage);
        }

        private void DisplayLogMessage(ConsoleLogEvent logEvent)
        {
            Debug.Log(logEvent.LogMessage);
        }
    }
}