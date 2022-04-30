using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public delegate void MethodContainer();
    public Sphere1T s1t;
    public Sphere2T s2t;

    private void Awake()
    {
        InputAggregator.OnTeleportEvent += s1t.TeleportUp;
        InputAggregator.OnTeleportEvent += s2t.TeleportDown;
    }
}
