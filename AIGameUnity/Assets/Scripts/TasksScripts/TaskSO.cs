using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Tasks/Task", fileName = "New task")]
public class TaskSO : ScriptableObject
{   //todo Может добавить название тасков
    [SerializeField] public string taskInfo = "Info about task"; 
    [SerializeField] public int firstAvailableDay = 0;
    [SerializeField] public int lastAvailableDay = 0;

    [SerializeField] public int priority = 100;

    //todo маркер задания?
    [SerializeField] string diaryInfo = "Info in diary after complete task";
}