using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableButton : InteractableBase
{
    Action callback;

    public void AssignUsedObject(Action execAction)
    {
        callback = execAction;
    }

    public override void OnInteract()
    {
        if (GameInfo.currentDevice.tag == "Bug") return;
        base.OnInteract();
        callback();
        EventAggregator.sceneButtonEvent.Publish();
    }

    public override bool CanInteract()
    {
        if (GameInfo.currentDevice.tag == "Bug") return false;
        return base.CanInteract();
    }
}
