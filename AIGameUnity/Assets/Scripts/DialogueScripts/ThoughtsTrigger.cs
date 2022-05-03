using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThoughtsTrigger : MonoBehaviour, ITrigger
{
    [SerializeField] private TextAsset inkFileSet;
    [Header("If it's a cutscene, please specify: ")]
    [SerializeField] private string cutsceneFolderNameSet;

    public TextAsset inkFile { get => inkFileSet; set => inkFileSet = value; }
    public string cutsceneFolderName { get => cutsceneFolderNameSet; set => cutsceneFolderNameSet = value; }

    private bool isThisInProgress = false;

    private void Awake()
    {
        EventAggregator.DialogueEnded.Subscribe(ThoughtsEnd);
    }

    private void OnTriggerEnter(Collider other)
    {
        StartThoughts(other);
    }

    private void OnTriggerStay(Collider other)
    {
        StartThoughts(other);
    }

    private void StartThoughts(Collider other)
    {
        if (other.gameObject == GameInfo.currentDevice)
        {
            isThisInProgress = true;
            GetComponent<BoxCollider>().enabled = false;

            EventAggregator.DialogueStarted.Publish(gameObject);
        }

    }

    private void ThoughtsEnd()
    {
        if (isThisInProgress)
        {
            isThisInProgress = false;
            gameObject.SetActive(false);
        }
    }
}
