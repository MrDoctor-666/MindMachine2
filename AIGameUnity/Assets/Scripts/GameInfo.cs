using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Characters
{
    Captain,
    FirstMate,
    Translator,
    Wife,
    Unknown
}

public enum PuzzleEnd
{
    Algorithm,
    Sabotage
}

public enum Endings
{
    TosterEnding,
    FinalEnding
}

public static class GameInfo
{
    public static float computingPower = 10;
    public static float suspicion = 10;
    public static GameObject currentDevice;
    public static DeviceInfo[] devices;
    public static bool isUsingMarker = true;
    public static Dictionary<string, Characters> people = new Dictionary<string, Characters>()
    {
        { "капитан", Characters.Captain},
        { "космо плав", Characters.Captain},
        { "первый помощник", Characters.FirstMate},
        { "переводчик", Characters.Translator },
        { "разно буквец", Characters.Translator },
        { "свет очей", Characters.Wife }
    };
 

    public static void DecreaseCompPower(float amount)
    {
        if ((computingPower - amount) >= 0) computingPower -= amount;
    }
    public static void IncreaseCompPower(float amount)
    {//todo Границы?
        computingPower += amount;
    }
    public static void DecreaseSuspicion(float amount)
    {
        if ((suspicion - amount) >= 0) suspicion -= amount;
        else suspicion = 0;
    }
    public static void IncreaseSuspicion(float amount)
    {
        suspicion += amount;
    }

    public static void Reset()
    {
        computingPower = 10;
        suspicion = 10;
        currentDevice = null;
        devices = null;
    }
}
