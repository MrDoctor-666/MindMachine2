using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeObjectTask : MonoBehaviour
{
    [SerializeField] private TaskSO taskSo;
    [SerializeField] private Roboarm roboarm;
    [SerializeField] private GameObject takenGameObject;

    private void Awake()
    {
        EventAggregator.takenObject.Subscribe(TakeObjectTaskComplete);
    }

    private void TakeObjectTaskComplete()
    {
        if (roboarm.objectToSave.Equals(takenGameObject))
        {
            EventAggregator.taskComplete.Publish(taskSo);
        }
    }
    
}
