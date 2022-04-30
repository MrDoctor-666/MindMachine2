using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithChangingUI : InteractableBase
{
    List<GameObject> uiToActivate;
    int uiNum = 0;

    private void Awake()
    {
        //can inherit this calss
        //and subscribe to events
        //that change UI
    }

    public override void OnInteract()
    {
        base.OnInteract();

        EventAggregator.PanelOpened.Publish(uiToActivate[uiNum]);
    }

    public void SwitchPanel()
    {
        if (uiToActivate.Count < uiNum + 1) uiNum++;
    }
}
