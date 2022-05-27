using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using TMPro;


public class TaskManager : MonoBehaviour
{
    [SerializeField] public List<TaskSO> taskList = new List<TaskSO>();
    [SerializeField] public List<TaskSO> allTasksList = new List<TaskSO>();

    private MyCompare<TaskSO> myCompare = new MyCompare<TaskSO>();

    [SerializeField] private TextMeshProUGUI showText;


    //todo проверка какие уст от каких зад зависят на добавление
    private void Awake()
    {
        EventAggregator.taskComplete.Subscribe(DeleteTask);
        EventAggregator.deleteFromTaskBank.Subscribe(DeleteTaskFromBank);
        EventAggregator.deleteFromTaskBank.Subscribe(DeleteTask);
        EventAggregator.newDayStarted.Subscribe(AddTaskForDay);
        EventAggregator.newDayStarted.Subscribe(DeleteOldTasks);
        EventAggregator.addTask.Subscribe(AddTask);
    }

    private void Start()
    {
        AddTaskForDay(1);
        SortList();
    }

    public void DeleteOldTasks(int newDay)
    {
        foreach (var task in taskList.ToList())
        {
            if (task.lastAvailableDay < newDay)
            {
                taskList.Remove(task);
            }
        }

        //todo удаление из банка, возможно и не надо
        foreach (var task in allTasksList.ToList())
        {
            if (task.firstAvailableDay < newDay)
            {
                allTasksList.Remove(task);
            }
        }
        ChangeMissionWaypoint(taskList[0]);

        ShowText();
    }

    public void DeleteTask(TaskSO taskSo)
    {
        print("Зашли в DeleteTask с заданием" + taskSo.name);
        //Пока пусть удаление будет происходить по ID, возможно можно будет просто делать удаление верхнего (pop) элемента, т.к. он активный только
        taskList.Remove(taskSo);
        EventAggregator.taskCompleteSound.Publish();
        SortList();
        print("В листе сейчас " + taskList.Count + " Задач после удаления!");
        if (taskList.Count>0)
        {
            
            ChangeMissionWaypoint(taskList[0]);

        }
        ShowText();
        //todo возможно надо делать сортировку после каждого удаление/добавление, но производительность...
    }
    public void DeleteTaskFromBank(TaskSO taskSo)
    {
        allTasksList.Remove(taskSo);
    }

    public void AddTask(TaskSO task)
    {
        taskList.Add(task);

        ShowText();
    }

    public void AddTaskForDay(int newDay)
    {
        foreach (var taskInBank in allTasksList)
        {
            if (taskInBank.firstAvailableDay == newDay)
            {
                taskList.Add(taskInBank);
            }
        }

        SortList();
        ChangeMissionWaypoint(taskList[0]);
        ShowText();
    }

    public void ShowText()
    {
        if (taskList.Count > 0)
        { 
            showText.enabled = true;
            showText.text = taskList[0].taskInfo;
        }
        else
        {
            showText.text = "Задач на сегодня нет.";
            //showText.enabled = false;
        }
    }

    public void SortList()
    {
        taskList.Sort(myCompare.Compare);
    }

    public void ChangeMissionWaypoint(TaskSO taskSo)
    {
        foreach (var taskTransform in tasksTransformDictionary)
        {
            if (taskTransform.taskSo.Equals(taskSo))
            {
                EventAggregator.changeMissionWaypoint.Publish(taskTransform.transform, taskTransform.offset);
            }
    
        }
    }

    [Serializable]
    public class DictionaryTaskTransform
    {
        public TaskSO taskSo;
        public Transform transform;
        public Vector3 offset;
    }
    
    [SerializeField] private DictionaryTaskTransform[] tasksTransformDictionary;
}

public class MyCompare<T> : IComparer<T> where T : TaskSO
{
    //todo сделать проверку на NULL объектов 
    public int Compare(T x, T y)
    {
        if (x.priority > y.priority)
        {
            return 1;
        }
        else if (x.priority < y.priority)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
}