using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTaskComplete : MonoBehaviour
{
    private void Awake()
    {
        EventAggregator.DialogueStarted.Subscribe(CompleteDialogue);
    }

    private void CompleteDialogue(GameObject obj)
    {
        if (obj.GetComponent<DialogueTrigger>())
        {
            foreach (var dictionaryTaskInteract in tasksDictionary)
            {
                if (obj.GetComponent<DialogueTrigger>().inkFile == dictionaryTaskInteract.inkFile)
                {
                    EventAggregator.taskComplete.Publish(dictionaryTaskInteract.taskSo);
                    //todo надо ли удалять задания из списка?
                }
            }
        }
    }

    [Serializable]
    public class DictionaryTaskDialogue
    {
        public TaskSO taskSo;
        public TextAsset inkFile;
    }

    [SerializeField] private DictionaryTaskDialogue[] tasksDictionary;
}