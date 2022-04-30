using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventOneParam<T>
{
    private readonly List<Action<T>> _callbacks = new List<Action<T>>();

    public void Subscribe(Action<T> callback)
    {
        _callbacks.Add(callback);
    }

    public void Publish(T obj)
    {
        foreach (Action<T> callback in _callbacks)
            callback(obj);
    }
}

public class EventNoParam
{
    private readonly List<Action> _callbacks = new List<Action>();

    public void Subscribe(Action callback)
    {
        _callbacks.Add(callback);
    }

    public void Publish()
    {
        foreach (Action callback in _callbacks)
            callback();
    }
}
