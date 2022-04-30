using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomicController : MonoBehaviour
{   //todo Возможно нужно переделать под 2 ивента: 1 ивент для взлома, другой для обычного выполнения задания. Оставляю так, потому что это гибкая реализация, пока не будет дизайна логики начисления/вычета ресурсов
    private TaskInfo taskInfo;
  private void Awake()
  { 
    EventAggregator.increaseCompPowerEvent.Subscribe(IncreaseCompPower);
    EventAggregator.decreaseCompPowerEvent.Subscribe(DecreaseCompPower);
    EventAggregator.increaseSuspicionEvent.Subscribe(IncreaseSuspicion);
    EventAggregator.decreaseSuspicionEvent.Subscribe(DecreaseSuspicion);
  }
    
  private void IncreaseCompPower(GameObject obj)
  {
      taskInfo = obj.GetComponent<TaskInfo>();
      GameInfo.IncreaseCompPower(taskInfo.compPowerIncrease);
      
  }
  private void DecreaseCompPower(GameObject obj)
  {
      taskInfo = obj.GetComponent<TaskInfo>();
      GameInfo.DecreaseCompPower(taskInfo.compPowerDecrease);
      
  }
  private void IncreaseSuspicion(GameObject obj)
  {
      taskInfo = obj.GetComponent<TaskInfo>();
      GameInfo.IncreaseSuspicion(taskInfo.suspicionIncrease);
      
  }
  private void DecreaseSuspicion(GameObject obj)
  {
      taskInfo = obj.GetComponent<TaskInfo>();
      GameInfo.DecreaseSuspicion(taskInfo.suspicionDecrease);
      
  }

}
