using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTask : MonoBehaviour
{
    [SerializeField] private TaskSO taskSo;
    [SerializeField] private string deviceTag;
    [SerializeField] private GameObject objectToDelivery;
    [SerializeField] private bool isDeliveryTask, isAllDevices;
   // [SerializeField] private DeliveryMove deliveryMove;
    private Inventory inventory;
    
    //todo сделать выбор из 3 Tag
    private void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Зашли в OnTriggerEnter");
        if (isAllDevices)
        {
            EventAggregator.taskComplete.Publish(taskSo);   
        }

        if (isDeliveryTask)
        { print("Зашли в DelTask");
            print("проверка инвентарь имя: " + inventory.objectToSaveOnDelivery.name);
            print("проверка в доставщике предмета имя: " + objectToDelivery.name);
            print("проверка тега у доставщика: " + other.CompareTag("DeliveryBot"));
            if (inventory.objectToSaveOnDelivery.name==objectToDelivery.name && other.CompareTag("DeliveryBot"))
            {
                print("перед таскКомплит");
             EventAggregator.taskComplete.Publish(taskSo);   
             Debug.Log("Доставщик привёз предмет");
             Debug.Log("Вещь в инвентаре: " + inventory.objectToSaveOnDelivery.name);
             Debug.Log("Вещь в задании на доставку: " + inventory.objectToSaveOnDelivery.name);
             Debug.Log("Название задания : " + taskSo.name);
            }
        }
        else
        {
            if (other.CompareTag(deviceTag))
            {
                EventAggregator.taskComplete.Publish(taskSo);
                Debug.Log(deviceTag + " прибыл в триггер");
            }
        }
    }
}