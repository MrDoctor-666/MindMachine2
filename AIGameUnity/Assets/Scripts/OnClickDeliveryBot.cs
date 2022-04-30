using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickDeliveryBot : InteractableBase
{
    private DeviceController deviceController;
    private void Awake()
    {
        deviceController = FindObjectOfType<DeviceController>();
    }
    public override void OnInteract()
    {
        //get current device
        GameObject device = deviceController.currentDevice;
        //probably should check it not by name
        if (device.name == "BugCocroach")
        {
            //put it in inventory
        }
        //switch to delivery bot
        EventAggregator.DeviceSwitched.Publish(gameObject);
    }
}
