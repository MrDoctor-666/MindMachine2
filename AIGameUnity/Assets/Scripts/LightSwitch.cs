using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    [SerializeField] Color blockedColor = Color.red;
    [SerializeField] Color openColor = Color.green;
    [Header("This should be 23 task (Intercat Chip)")]
    [SerializeField] TaskSO taskCompletedSwitch;

    private List<Light> lights = new List<Light>();

    private void Awake()
    {
        lights = GetComponentsInChildren<Light>().ToList();
        foreach (var light in lights)
            light.color = blockedColor;

        EventAggregator.taskComplete.Subscribe(SwitchLights);
    }

    private void SwitchLights(TaskSO obj)
    {
        if (!obj.Equals(taskCompletedSwitch)) return;
        Debug.Log("Lights Switched");
        foreach (var light in lights)
            light.color = openColor;
    }
}
