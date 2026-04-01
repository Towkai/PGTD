using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

namespace EventDispatcher
{
    public class Dispatcher
    {
        private static readonly Lazy<Dispatcher> _instance = new(() => new Dispatcher());
        public static Dispatcher Instance => _instance.Value;

        private readonly Dictionary<Type, List<Delegate>> _eventDictionary = new();

        private Dispatcher()
        {
            
        }

        public void Subscribe<T>(Action<T> handler) where T: IEvent
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler), "The handler method cannot be null");

            if (_eventDictionary.TryGetValue(typeof(T), out List<Delegate> existingHandlers))
            {
                if (existingHandlers.Contains(handler))
                {
                    string errorMessage = $"The handler '{handler.Method.Name}' is already subscribed to the event '{typeof(T)}'.";
                    throw new InvalidOperationException(errorMessage);
                }

                existingHandlers.Add(handler);
            }
            else
            {
                _eventDictionary[typeof(T)] = new List<Delegate> {handler};
            }
        }

        public void Unsubscribe<T>(Action<T> handler) where T: IEvent
        {
            if (handler == null) return;

            if (_eventDictionary.TryGetValue(typeof(T), out List<Delegate> existingHandlers))
            {
                existingHandlers.Remove(handler);
                if (existingHandlers.Count == 0)
                {
                    _eventDictionary.Remove(typeof(T));
                }
            }
        }

        public void Dispatch<T>(T payload) where T: IEvent
        {
            if (_eventDictionary.TryGetValue(typeof(T), out List<Delegate> handlers))
            {
                var snapshot = handlers.ToArray(); //使用快取避免執行過程中 handlers 產生變動(取消註冊)
                foreach (Delegate handler in snapshot)
                {
                    try
                    {
                        ((Action<T>) handler)?.Invoke(payload);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Error occurred while dispatching the event: {e.Message}");
                    }
                }
            }
            else
            {
                Debug.LogWarning($"No subscribers for event type: {typeof(T)}");
            }
        }

        public void UnsubscribeAll<T>() where T: IEvent
        {
            _eventDictionary.Remove(typeof(T));
        }

        public void Clear()
        {
            _eventDictionary.Clear();
        }

        public void Dispatch<T>(params T[] events) where T: IEvent
        {
            foreach (var eventPayload in events)
            {
                Dispatch(eventPayload);
            }
        }
    }
}