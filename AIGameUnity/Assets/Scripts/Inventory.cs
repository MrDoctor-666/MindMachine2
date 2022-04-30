using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("The order of the objects must be the same!!!")]
    [SerializeField] private List<GameObject> canBePutOn;
    [SerializeField] private List<Sprite> canBePutOnSprites;
    [HideInInspector][SerializeField] private List<Roboarm> roboarms;

    //todo заменить на поиск на сцене и автоматическое назначение
    [SerializeField] private Image inventoryImage;
    [SerializeField] private Text inventoryText;
    private DeliveryMove deliveryMove;


    public GameObject objectToSaveOnDelivery;

    public Dictionary<int, GameObject> dictionary = new Dictionary<int, GameObject>();

    
    private void Awake()
    {
        //todo сделать по-другому
        inventoryImage.enabled = false;
        EventAggregator.inventoryImageEvent.Subscribe(ChangeImage);
        EventAggregator.DeviceSwitched.Subscribe(ChangeImage);
        roboarms.AddRange(FindObjectsOfType<Roboarm>());
        for (int i = 0; i < roboarms.Count; i++)
        {
            // 1 - null
            // 2 - null
            Debug.Log("У руки ID = " + roboarms[i].id);

            dictionary.Add(roboarms[i].id, null);
        }
    }

    public void ChangeImage(GameObject obj)
    {
        if (obj.GetComponent<DeliveryMove>())
        {
            if (objectToSaveOnDelivery != null)
            {
                SpriteOfImage(objectToSaveOnDelivery.name);
                inventoryImage.enabled = true;
            }
            else
            {
                inventoryImage.enabled = false;
            }
        }
        else if (obj.GetComponent<Roboarm>())
        {
            if (dictionary[obj.GetComponent<Roboarm>().id] != null)
            {
                SpriteOfImage(dictionary[obj.GetComponent<Roboarm>().id].name);
                inventoryImage.enabled = true;
            }
            else
            {
                inventoryImage.enabled = false;
            }
        }
        else
        {
            inventoryImage.enabled = false;
        }
    }

    public void SpriteOfImage(string item)
    {
        for (int i = 0; i < canBePutOn.Count; i++)
        {
            if (canBePutOn[i].name == item)
            {
                Debug.Log(item);
                inventoryImage.sprite = canBePutOnSprites[i];
            }
        }
    }
}