using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTaskAfterThought : MonoBehaviour
{
    [SerializeField] private TaskSO taskSo;
    [SerializeField] private TextAsset inkFile;
    
    private void Awake()
    {
        EventAggregator.DialogueStarted.Subscribe(CompleteThought);
    }

    private void CompleteThought(GameObject gameObject)
    {
        if (gameObject.GetComponent<ThoughtsTrigger>())
        {
            if (gameObject.GetComponent<ThoughtsTrigger>().inkFile == inkFile)
            {
                EventAggregator.addTask.Publish(taskSo);
            }
        }
       
    }
}
