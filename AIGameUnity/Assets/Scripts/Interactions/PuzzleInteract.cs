using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleInteract : InteractableBase, IPuzzled
{
    [Header("Puzzle Settings")]
    [SerializeField] private float prizeSabotge;
    [SerializeField] private float suspicionSabotage;
    [SerializeField] private float minusSuspitionAlgorithm;

    [Header("IF YOU NEED CUTSCENE AFTER")]
    [SerializeField] GameObject algorithmEnd;
    [SerializeField] GameObject sabotageEnd;

    float IPuzzled.prizeSabotge { get => prizeSabotge; set => prizeSabotge = value; }
    float IPuzzled.suspicionSabotage { get => suspicionSabotage; set => suspicionSabotage = value; }
    float IPuzzled.minusSuspitionAlgorithm { get => minusSuspitionAlgorithm; set => minusSuspitionAlgorithm = value; }

    bool isStarted = false;

    private void Awake()
    {
        EventAggregator.puzzleEnded.Subscribe(OnPuzzleEnd);
    }

    private void OnPuzzleEnd(PuzzleEnd obj)
    {
        Debug.Log(isStarted);
        if (isStarted)
        {
            isStarted = false;
            StartCoroutine(wait(obj));
        }
    }

    public override void OnInteract()
    {
        base.OnInteract();
        isStarted = true;
        EventAggregator.puzzleStarted.Publish(gameObject);
    }

    IEnumerator wait(PuzzleEnd puzzleEnd)
    {
        yield return new WaitForSeconds(0.5f);
        if (puzzleEnd == PuzzleEnd.Algorithm && algorithmEnd != null)
            EventAggregator.DialogueStarted.Publish(algorithmEnd);
        else if (puzzleEnd == PuzzleEnd.Sabotage && sabotageEnd != null)
            EventAggregator.DialogueStarted.Publish(sabotageEnd);
    }
}
