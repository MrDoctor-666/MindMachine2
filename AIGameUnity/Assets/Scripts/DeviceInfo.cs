using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceInfo : MonoBehaviour
{
    public string nameToDisplay;
    public bool isActive = false;
    public bool isBlocked;
    public float cost;
    [Header("Computing Power")]
    //public float prizeAlgorithm;
    public float prizeSabotge;
    public float everyDayPrize;
    [Header("Suspition")]
    public float suspicionSabotage;
    public float minusSuspitionAlgorithm;

    //public GameObject puzzle;

    //maybe we can make this an abstract class
    //and inherit it to CockroachInfo or something
}
