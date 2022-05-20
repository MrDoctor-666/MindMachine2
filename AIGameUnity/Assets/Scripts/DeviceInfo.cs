using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceInfo : MonoBehaviour, IPuzzled
{
    public string nameToDisplay;
    public bool isActive = false;
    public bool isBlocked;
    public float cost;
    [Header("Computing Power")]
    //public float prizeAlgorithm;
    public float everyDayPrize;
    [Header("Puzzle Settings")]
    [SerializeField] private float compPowerAlgorithm;
    [SerializeField] private float suspicionSabotage;
    //[SerializeField] private float minusSuspitionAlgorithm;

    float IPuzzled.compPowerAlgorithm { get => compPowerAlgorithm; set => compPowerAlgorithm = value; }
    float IPuzzled.suspicionSabotage { get => suspicionSabotage; set => suspicionSabotage = value; }
    //float IPuzzled.minusSuspitionAlgorithm { get => minusSuspitionAlgorithm; set => minusSuspitionAlgorithm = value; }

    //maybe we can make this an abstract class
    //and inherit it to CockroachInfo or something
}
