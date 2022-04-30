using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TaskHack : MonoBehaviour
{
    private void Awake()
    {
        EventAggregator.puzzleStarted.Subscribe(CompleteHack);
    }

    private void CompleteHack(GameObject obj)
    {
        foreach (var dictionaryTaskInteract in tasksDictionary)
        {
            if (obj == dictionaryTaskInteract.objectToHack)
            {
                EventAggregator.taskComplete.Publish(dictionaryTaskInteract.taskSo);
                //todo надо ли удалять задания из списка?
            }
        }
    }
    [Serializable]
    public class DictionaryTaskHack
    {
        public TaskSO taskSo;
        public GameObject objectToHack;
    }
    
    [SerializeField] private DictionaryTaskHack[] tasksDictionary;
}