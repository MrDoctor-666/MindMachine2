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
            Debug.Log("CompleteDialogue in DialogueTaskComplete: " + obj);
            Debug.Log("Ink File: " + obj.GetComponent<DialogueTrigger>().inkFile);
            foreach (var dictionaryTaskInteract in tasksDictionary)
            {
                Debug.Log("== in foreach by inkfile " + (obj.GetComponent<DialogueTrigger>().inkFile == dictionaryTaskInteract.inkFile));
                Debug.Log("== in foreach by name " + (obj.GetComponent<DialogueTrigger>().inkFile.name == dictionaryTaskInteract.inkFile.name));
                Debug.Log("== in foreach by text " + (obj.GetComponent<DialogueTrigger>().inkFile.text == dictionaryTaskInteract.inkFile.text));

                if (obj.GetComponent<DialogueTrigger>().inkFile.name == dictionaryTaskInteract.inkFile.name ||
                    obj.GetComponent<DialogueTrigger>().inkFile == dictionaryTaskInteract.inkFile)
                {
                    Debug.Log("Dialogue Task Complete Published");
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