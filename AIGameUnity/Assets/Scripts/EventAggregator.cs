using System;
using UnityEngine;

public class EventAggregator
{
    public static EventOneParam<GameObject> DeviceSwitched = new EventOneParam<GameObject>();
    public static EventOneParam<GameObject> IntercationAreaEntered = new EventOneParam<GameObject>();
    public static EventOneParam<GameObject> IntercationAreaExited = new EventOneParam<GameObject>();
    public static EventOneParam<GameObject> DeviceBuyTried = new EventOneParam<GameObject>();
    public static EventOneParam<GameObject> DeviceBuyError = new EventOneParam<GameObject>();
    public static EventNoParam deviceBought = new EventNoParam();

    //change day system
    public static EventOneParam<int> newDayStarted = new EventOneParam<int>();
    public static EventNoParam changeDay = new EventNoParam();


    //when open/closing some ui
    public static EventOneParam<GameObject> PanelOpened = new EventOneParam<GameObject>();
    public static EventNoParam PanelClosed = new EventNoParam();
    public static EventOneParam<string> TempPanelOpened = new EventOneParam<string>();
    public static EventNoParam clickButtonUI = new EventNoParam();

    //inventory
    public static EventOneParam<GameObject> inventoryImageEvent = new EventOneParam<GameObject>();
    public static EventOneParam<GameObject> takeObjectEvent = new EventOneParam<GameObject>();
    public static EventNoParam getObjectEvent = new EventNoParam();
    public static EventNoParam putObjectEvent = new EventNoParam();

    //dialogues
    public static EventOneParam<GameObject> DialogueStarted = new EventOneParam<GameObject>();
    public static EventNoParam DialogueEnded = new EventNoParam();
    
    //economics
    public static EventOneParam<GameObject> increaseCompPowerEvent = new EventOneParam<GameObject>();
    public static EventOneParam<GameObject> decreaseCompPowerEvent = new EventOneParam<GameObject>();
    public static EventOneParam<GameObject> increaseSuspicionEvent = new EventOneParam<GameObject>();
    public static EventOneParam<GameObject> decreaseSuspicionEvent = new EventOneParam<GameObject>();

    //puzzles
    public static EventOneParam<GameObject> puzzleStarted = new EventOneParam<GameObject>();
    public static EventNoParam snakeMoving = new EventNoParam();
    public static EventNoParam pushButton = new EventNoParam();
    public static EventNoParam getLight = new EventNoParam();
    public static EventNoParam doorOpened = new EventNoParam();
    public static EventOneParam<PuzzleEnd> puzzleEnded = new EventOneParam<PuzzleEnd>();

    //moving device
    public static EventOneParam<GameObject> startMoving = new EventOneParam<GameObject>();
    public static EventOneParam<GameObject> endMoving = new EventOneParam<GameObject>();

    //cockroach
    public static EventNoParam cockroachJump = new EventNoParam();

    //buttons
    public static EventNoParam changeWayEvent = new EventNoParam();
    public static EventNoParam sceneButtonEvent = new EventNoParam();

    //doors
    public static EventNoParam sceneDoorsEvent = new EventNoParam();

    //toaster
    public static EventNoParam toasterEvent = new EventNoParam();

    //elevator
    public static EventNoParam liftMovingEvent = new EventNoParam();
   
    //tasks
    public static EventOneParam<TaskSO> taskComplete = new EventOneParam<TaskSO>();
    public static EventOneParam<TaskSO> deleteFromTaskBank = new EventOneParam<TaskSO>();
    public static EventOneParam<int> taskListAdd = new EventOneParam<int>();
    public static EventNoParam takenObject = new EventNoParam();
    public static EventOneParam<TaskSO> addTask = new EventOneParam<TaskSO>();
    public static EventTwoParam<Transform, Vector3> changeMissionWaypoint = new EventTwoParam<Transform, Vector3>();

    //endGame
    public static EventOneParam<Endings> endGame = new EventOneParam<Endings>();

    public static void Reset()
    {
        Debug.Log("RESET EVENT AGGREGATOR");
        Type type = typeof(EventAggregator);
        foreach(var p in type.GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public))
        {
            if (p.GetValue(null).GetType() == typeof(EventNoParam))
                (p.GetValue(null) as EventNoParam).Clear();
            else if (p.GetValue(null).GetType().IsGenericType)
            {
                Type[] types = p.GetValue(null).GetType().GetGenericArguments();
                (p.GetValue(null) as AnyEvent).Clear();
            }
        }
    }
}
