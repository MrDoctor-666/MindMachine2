using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickDevice : InteractableBase
{
    private void Awake()
    {
        if (GetComponent<DeviceInfo>().isBlocked)
            SwitchTooltipMessage("Blocked. Cost: " + GetComponent<DeviceInfo>().cost);
        else SwitchTooltipMessage("Switch");
    }
    public override void OnInteract()
    {
        if (gameObject.GetComponent<DeviceInfo>().isBlocked == false)
            EventAggregator.DeviceSwitched.Publish(gameObject);
        else
        {
            if (gameObject.GetComponent<DeviceInfo>().cost > GameInfo.computingPower)
                EventAggregator.DeviceBuyError.Publish(gameObject);
            else
                EventAggregator.DeviceBuyTried.Publish(gameObject);
        }
    }
}
