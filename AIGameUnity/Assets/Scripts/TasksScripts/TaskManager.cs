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
    [SerializeField] List<TaskSO> allTasksList = new List<TaskSO>();

    private MyCompare<TaskSO> myCompare = new MyCompare<TaskSO>();

    //[SerializeField] private GameObject questPanel;
    [SerializeField] private TextMeshProUGUI showText;


    //todo проверка какие уст от каких зад зависят на добавление
    private void Awake()
    {
        EventAggregator.taskComplete.Subscribe(DeleteTask);
        EventAggregator.newDayStarted.Subscribe(AddTaskForDay);
        EventAggregator.newDayStarted.Subscribe(DeleteOldTasks);
        EventAggregator.addTask.Subscribe(AddTask);
        AddTaskForDay(1);
    }

    private void Start()
    {
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

        ShowText();
    }

    public void DeleteTask(TaskSO taskSo)
    {
        //Пока пусть удаление будет происходить по ID, возможно можно будет просто делать удаление верхнего (pop) элемента, т.к. он активный только
        taskList.Remove(taskSo);
        SortList();

        ShowText();
        //todo возможно надо делать сортировку после каждого удаление/добавление, но производительность...
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

        ShowText();
    }

    public void ShowText()
    {
        //todo масштабирование текста
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