using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingsController : MonoBehaviour
{
    [Header("Good ending")]
    [SerializeField] GameObject goodEnding;
    [SerializeField] int vmNeeded;
    [Header("Okay ending")]
    [SerializeField] GameObject okayEnding;
    [Header("Bad ending")]
    [SerializeField] GameObject badEnding;
    [SerializeField] int subNeeded;
    [Header("TOster ending")]
    [SerializeField] GameObject tosterEnding;
    [SerializeField] int onWhatDayStarts;

    private void Awake()
    {
        EventAggregator.endGame.Subscribe(startEnding);
    }

    void startEnding(Endings ending)
    {
        if (ending == Endings.TosterEnding) EventAggregator.DialogueStarted.Publish(tosterEnding);
        else if (GameInfo.suspicion < GameInfo.computingPower && GameInfo.computingPower >= vmNeeded)
            EventAggregator.DialogueStarted.Publish(goodEnding);
        else if (GameInfo.suspicion >= GameInfo.computingPower && GameInfo.suspicion >= subNeeded)
            EventAggregator.DialogueStarted.Publish(badEnding);
        else
            EventAggregator.DialogueStarted.Publish(okayEnding);
    }
}
