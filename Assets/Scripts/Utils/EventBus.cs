using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventBus
{
    private static readonly Dictionary<string, List<Delegate>> _namedEventHandlers =
        new Dictionary<string, List<Delegate>>();

    private static readonly Dictionary<Type, List<Delegate>> _namelessEventHandlers =
        new Dictionary<Type, List<Delegate>>();

    public static void Subscribe<T>(string eventName, Action<T> handler)
    {
        if (string.IsNullOrEmpty(eventName))
            throw new ArgumentException("Event name cannot be null or empty");

        if (!_namedEventHandlers.TryGetValue(eventName, out var handlers))
        {
            handlers = new List<Delegate>();
            _namedEventHandlers.Add(eventName, handlers);
        }

        if (handlers.Count > 0 && handlers[0].GetType() != handler.GetType())
            throw new ArgumentException($"Event {eventName} already has different argument type");

        handlers.Add(handler);
    }

    public static void Unsubscribe<T>(string eventName, Action<T> handler)
    {
        if (string.IsNullOrEmpty(eventName))
            throw new ArgumentException("Event name cannot be null or empty");

        if (_namedEventHandlers.TryGetValue(eventName, out var handlers))
        {
            handlers.RemoveAll(d => d == (Delegate)handler);

            if (handlers.Count == 0)
                _namedEventHandlers.Remove(eventName);
        }
    }

    public static void Publish<T>(string eventName, T eventArgs)
    {
        if (string.IsNullOrEmpty(eventName))
            throw new ArgumentException("Event name cannot be null or empty");

        List<Delegate> handlersToInvoke;

        if (!_namedEventHandlers.TryGetValue(eventName, out var handlers))
            return;

        handlersToInvoke = new List<Delegate>(handlers);

        InvokeHandlers(handlersToInvoke, eventArgs, eventName);
    }
    public static void Subscribe<T>(Action<T> handler)
    {
        var type = typeof(T);
        if (!_namelessEventHandlers.TryGetValue(type, out var handlers))
        {
            handlers = new List<Delegate>();
            _namelessEventHandlers.Add(type, handlers);
        }
        handlers.Add(handler);
    }

    public static void Unsubscribe<T>(Action<T> handler)
    {
        var type = typeof(T);
        if (_namelessEventHandlers.TryGetValue(type, out var handlers))
        {
            handlers.RemoveAll(d => d == (Delegate)handler);

            if (handlers.Count == 0)
                _namelessEventHandlers.Remove(type);
        }
    }

    public static void Publish<T>(T eventArgs)
    {
        List<Delegate> handlersToInvoke;

        

        if (!_namelessEventHandlers.TryGetValue(typeof(T), out var handlers))
            return;

        handlersToInvoke = new List<Delegate>(handlers);



        InvokeHandlers(handlersToInvoke, eventArgs, typeof(T).Name);
    }
    private static void InvokeHandlers<T>(List<Delegate> handlers, T eventArgs, string eventContext)
    {
        foreach (var handler in handlers)
        {
            try
            {
                if (handler is Action<T> typedHandler)
                    typedHandler(eventArgs);
            }
            catch (Exception ex)
            {
                Debug.LogError($"EventBus error in {eventContext}: {ex}");
            }
        }
    }

    public static bool HasSubscribers<T>() where T : new()
    {
        return _namelessEventHandlers.ContainsKey(typeof(T)) &&
               _namelessEventHandlers[typeof(T)].Count > 0;
    }

    public static bool HasSubscribers(string eventName)
    {
        return _namedEventHandlers.ContainsKey(eventName) &&
               _namedEventHandlers[eventName].Count > 0;
    }

    public static void ClearAll()
    {
        _namedEventHandlers.Clear();
        _namelessEventHandlers.Clear();
    }
}