using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//so this has specific ObjectOnSceneTest script
//so we can get some parameters from there
public class AddCurrencyEvent
{
    private readonly List<Action<ObjectOnSceneTest>> _callbacks = new List<Action<ObjectOnSceneTest>>();

    public void Subscribe(Action<ObjectOnSceneTest> callback)
    {
        _callbacks.Add(callback);
    }

    public void Publish(ObjectOnSceneTest unit)
    {
        foreach (Action<ObjectOnSceneTest> callback in _callbacks)
            callback(unit);
    }
}

//and this has MonoBehaviour, so we add parameters (int)
//i dont like it, but we can get 2 or 3 parameters easily
public class AddCurrencyWithParametrsEvent
{
    private readonly List<Action<MonoBehaviour, int>> _callbacks = new List<Action<MonoBehaviour, int>>();

    public void Subscribe(Action<MonoBehaviour, int> callback)
    {
        _callbacks.Add(callback);
    }

    public void Publish(MonoBehaviour obj, int someParam)
    {
            foreach (Action<MonoBehaviour, int> callback in _callbacks)
                callback(obj, someParam);
    }
}

public class CubesDestroyedEvent
{
    private readonly List<Action<MonoBehaviour>> _callbacks = new List<Action<MonoBehaviour>>();

    public void Subscribe(Action<MonoBehaviour> callback)
    {
        _callbacks.Add(callback);
    }

    public void Publish(MonoBehaviour obj)
    {
        foreach (Action<MonoBehaviour> callback in _callbacks)
            callback(obj);
    }
}
