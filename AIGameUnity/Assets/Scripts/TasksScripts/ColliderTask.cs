using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTask : MonoBehaviour
{
    [SerializeField] private TaskSO taskSo;
    [SerializeField] private string deviceTag;
    [SerializeField] private GameObject objectToDelivery;
    [SerializeField] private bool isDeliveryTask;
   // [SerializeField] private DeliveryMove deliveryMove;
    private Inventory inventory;
    
    //todo сделать выбор из 3 Tag
    private void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDeliveryTask)
        {
            if (inventory.objectToSaveOnDelivery
                .Equals(objectToDelivery) && other.CompareTag("DeliveryBot"))
            {
             EventAggregator.taskComplete.Publish(taskSo);   
             Debug.Log("Доставщик привёз предмет");
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