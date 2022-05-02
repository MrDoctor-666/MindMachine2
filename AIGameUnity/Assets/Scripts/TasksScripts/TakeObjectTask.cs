using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeObjectTask : MonoBehaviour
{
    [SerializeField] private TaskSO taskSo;
    [SerializeField] private Roboarm roboarm;
    private Inventory inventory;
    [SerializeField] private GameObject takenGameObject;

    private void Awake()
    {   inventory = FindObjectOfType<Inventory>();
        EventAggregator.takenObject.Subscribe(TakeObjectTaskComplete);
    }

    private void TakeObjectTaskComplete()
    {
        if (inventory.dictionary[roboarm.id].Equals(takenGameObject))
        {
            EventAggregator.taskComplete.Publish(taskSo);
        }
    }
    
}
