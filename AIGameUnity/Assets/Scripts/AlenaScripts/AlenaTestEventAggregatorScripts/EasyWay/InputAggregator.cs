using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputAggregator : MonoBehaviour
{
    public static event EventController.MethodContainer OnTeleportEvent;

    private void Update()
    {
        if (Input.GetKeyDown("space")) OnTeleportEvent();
    }
}
