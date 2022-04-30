using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithUI : InteractableBase
{
    [SerializeField] GameObject uiToActicate;
    public override void OnInteract()
    {
        base.OnInteract();

        EventAggregator.PanelOpened.Publish(uiToActicate);
    }
}
