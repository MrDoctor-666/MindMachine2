using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endPanelInteract : InteractableBase
{
    public override void OnInteract()
    {
        //TODO() check if all tasks are done and it's the right day

        base.OnInteract();
        EventAggregator.endGame.Publish(Endings.FinalEnding);
        Debug.Log("END GAME!!");
    }

    public override bool CanInteract()
    {
        //TODO() if tasks not done return

        return base.CanInteract();
    }

}
