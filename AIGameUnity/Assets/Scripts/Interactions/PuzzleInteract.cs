using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleInteract : InteractableBase, IPuzzled, ITrigger
{
    [Header("Puzzle Settings")]
    [SerializeField] private float prizeSabotge;
    [SerializeField] private float suspicionSabotage;
    [SerializeField] private float minusSuspitionAlgorithm;

    [Header("IF YOU NEED CUTSCENE AFTER")]
    [SerializeField] private TextAsset inkFileSet;
    [Header("If it's a cutscene, please specify: ")]
    [SerializeField] private string cutsceneFolderNameSet;

    float IPuzzled.prizeSabotge { get => prizeSabotge; set => prizeSabotge = value; }
    float IPuzzled.suspicionSabotage { get => suspicionSabotage; set => suspicionSabotage = value; }
    float IPuzzled.minusSuspitionAlgorithm { get => minusSuspitionAlgorithm; set => minusSuspitionAlgorithm = value; }

    public TextAsset inkFile { get => inkFileSet; set => inkFileSet = value; }
    public string cutsceneFolderName { get => cutsceneFolderNameSet; set => cutsceneFolderNameSet = value; }

    bool isStarted = false;

    private void Awake()
    {
        EventAggregator.puzzleEnded.Subscribe(OnPuzzleEnd);
    }

    private void OnPuzzleEnd(PuzzleEnd obj)
    {
        if (isStarted && inkFile != null)
        {
            EventAggregator.DialogueStarted.Publish(gameObject);
            isStarted = false;
        }
    }

    public override void OnInteract()
    {
        base.OnInteract();
        isStarted = true;
        EventAggregator.puzzleStarted.Publish(gameObject);
    }
}
