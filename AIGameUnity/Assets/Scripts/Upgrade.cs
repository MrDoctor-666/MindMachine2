using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : InteractableBase
{
    public GameObject upgrade;
    public override void OnInteract()
    {
        base.OnInteract();

        upgrade.SetActive(true);
        Destroy(gameObject);
    }
}
