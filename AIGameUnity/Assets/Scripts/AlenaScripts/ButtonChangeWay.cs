using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChangeWay : InteractableBase
{
    [SerializeField] private Roboarm roboarm;
    public override void OnInteract()
    {
        base.OnInteract();
        roboarm.buttonChangeWay = true;
    }
}