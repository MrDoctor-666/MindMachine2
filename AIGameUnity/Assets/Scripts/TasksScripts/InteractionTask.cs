using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionTask : MonoBehaviour
{
    private void Awake()
    {
        EventAggregator.IntercationAreaExited.Subscribe(InteractTaskComplete);
    }

    private void InteractTaskComplete(GameObject obj)
    {
        foreach (var dictionaryTaskInteract in tasksDictionary)
        {
            if (obj == dictionaryTaskInteract.objectToInteract)
            {
                EventAggregator.taskComplete.Publish(dictionaryTaskInteract.taskSo);
                //todo надо ли удалять задания из списка?
            }
        }
    }
     [Serializable]
       public class DictionaryTaskInteract
       {
           public TaskSO taskSo;
           public GameObject objectToInteract;
       }
    
       [SerializeField] private DictionaryTaskInteract[] tasksDictionary;
     
}
