using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDialogueTrigger : InteractableBase, ITrigger
{
    [SerializeField] private TextAsset inkFileSet;
    [Header("If it's a cutscene, please specify: ")]
    [SerializeField] private string cutsceneFolderNameSet;

    public TextAsset inkFile { get => inkFileSet; set => inkFileSet = value; }
    public string cutsceneFolderName { get => cutsceneFolderNameSet; set => cutsceneFolderNameSet = value; }

    public override void OnInteract()
    {
        base.OnInteract();
        EventAggregator.DialogueStarted.Publish(gameObject);
        Debug.Log(gameObject);
    }
}
