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
    [SerializeField] public string diaryInfo = "Info in diary after complete task";

    // override object.Equals
    public override bool Equals(object obj)
    {
        //       
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237  
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //

        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        TaskSO objT = obj as TaskSO;
        // TODO: write your implementation of Equals() here
        return (taskInfo == objT.taskInfo && firstAvailableDay == objT.firstAvailableDay &&
            lastAvailableDay == objT.lastAvailableDay && priority == objT.priority &&
            diaryInfo == objT.diaryInfo);
        return base.Equals(obj);
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        // TODO: write your implementation of GetHashCode() here
        return base.GetHashCode();
    }
}