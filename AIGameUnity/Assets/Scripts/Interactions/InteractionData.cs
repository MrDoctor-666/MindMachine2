using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InteractionData
{
    public static InteractableBase interactionObj = null;

    public static void Reset()
    {
        interactionObj = null;
    }
}
