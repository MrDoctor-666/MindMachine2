using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class DialogueTrigger : InteractableBase, ITrigger
{
    [SerializeField] private TextAsset inkFileSet;
    [Header("If it's a cutscene, please specify: ")]
    [SerializeField] private string cutsceneFolderNameSet;

    public TextAsset inkFile { get => inkFileSet; set => inkFileSet = value; }
    public string cutsceneFolderName { get => cutsceneFolderNameSet; set => cutsceneFolderNameSet = value; }

    //[SerializeField] bool isThisEndDialogue = false;
    // bool isThisStarted = false;
    private void Awake()
    {
        //if (isThisEndDialogue) EventAggregator.DialogueEnded.Subscribe(End);
    }

    public override void OnInteract()
    {
        if (GameInfo.currentDevice.tag == "Bug")
        {
            //isThisStarted = true;
            base.OnInteract();
            EventAggregator.DialogueStarted.Publish(gameObject);
            Debug.Log(gameObject);
        }
        else EventAggregator.TempPanelOpened.Publish("Данное устройство не обладает запрошенным функционалом");
            //Debug.Log("Can't interact from others"); //todo write on ui that we can't interact
    }

    public override bool CanInteract()
    {
        return GameInfo.currentDevice.tag == "Bug";
    }

    /*void End()
    {
        //TODO DELETE IT LATER
        if (isThisStarted)
        {
            EventAggregator.endGame.Publish();
            isThisStarted = false;
        }
    }*/
}
