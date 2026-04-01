
using System;
using Interfaces;

namespace EventDispatcher
{
    public class ConsoleLogEvent : IEvent
    {
        public string LogMessage;

        public ConsoleLogEvent(string logMessage)
        {
            LogMessage = logMessage;
        }
    }
}