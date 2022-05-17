using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface AnyEvent
{
    public void Clear();
}
public class EventOneParam<T> : AnyEvent
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

    public void Clear()
    {
        _callbacks.Clear();
    }
}
public class EventTwoParam<T, V> : AnyEvent
{
    private readonly List<Action<T, V>> _callbacks = new List<Action<T, V>>();

    public void Subscribe(Action<T, V> callback)
    {
        _callbacks.Add(callback);
    }

    public void Publish(T obj, V obj2)
    {
        foreach (Action<T, V> callback in _callbacks)
            callback(obj, obj2);
    }

    public void Clear()
    {
        _callbacks.Clear();
    }
}

public class EventNoParam : AnyEvent
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

    public void Clear()
    {
        _callbacks.Clear();
    }
}
