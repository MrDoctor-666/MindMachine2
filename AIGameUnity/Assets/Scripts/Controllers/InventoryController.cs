using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    private Roboarm roboArmScript;
    private InteractableBase interact;
    private DeviceInfo deviceInfo;
    private Inventory inventory;
    private Vector3 sizeOfObject;
    private void Awake()
    {
        roboArmScript = gameObject.GetComponent<Roboarm>();
        deviceInfo = GetComponent<DeviceInfo>();
        EventAggregator.takeObjectEvent.Subscribe(TakeObjectFromScene);
        inventory = FindObjectOfType<Inventory>();
    }

    private void TakeObjectFromScene(GameObject obj)
    {
        if (deviceInfo.isActive)
        {
            int layerMask1 = 1 << 8;
            int layerMask2 = 1 << 9;
            layerMask1 = layerMask1 | layerMask2;
            layerMask1 = ~layerMask1;
            Ray ray = gameObject.GetComponentInChildren<Camera>()
                .ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.yellow);
            if (Physics.Raycast(ray, out hit, roboArmScript.maxInteractDistance, layerMask1))
            {
                interact = hit.transform.GetComponent<InteractableBase>();
                if (interact == null)
                {
                    return;
                }

                if (inventory.dictionary[roboArmScript.id] == null &&
                    !hit.collider.gameObject.CompareTag("DeliveryBot") &&
                    interact.IsPortable)
                {
                    inventory.dictionary[roboArmScript.id] = hit.collider.gameObject;
                    sizeOfObject = hit.collider.bounds.size;
                    inventory.dictionary[roboArmScript.id].SetActive(false);
                    EventAggregator.takenObject.Publish();
                    //публикация взятия объекта для задания
                    EventAggregator.getObjectEvent.Publish();
                    roboArmScript.isArmEmpty = false;
                    Debug.Log("Взяли объект");
                }

                // пробуем взять объект из доставщика 
                else if (inventory.dictionary[roboArmScript.id] == null &&
                         hit.collider.gameObject.CompareTag("DeliveryBot"))
                {
                    Debug.Log("Есть вещь" + inventory.dictionary[roboArmScript.id] + " ID: " + roboArmScript.id);
                    //"забираем" объект у доставщика
                    if (inventory.objectToSaveOnDelivery != null)
                    {
                        inventory.dictionary[roboArmScript.id] = inventory.objectToSaveOnDelivery;
                        inventory.objectToSaveOnDelivery = null;
                        EventAggregator.getObjectEvent.Publish();
                        roboArmScript.isArmEmpty = false;
                        Debug.Log("Забрали вещь из доставщика");
                    }
                }
                // пробуем положить вещь в доставщика
                else if (inventory.dictionary[roboArmScript.id] != null &&
                         hit.collider.gameObject.CompareTag("DeliveryBot"))
                {
                    Debug.Log("нет вещи " + inventory.dictionary[roboArmScript.id] + " ID: " + roboArmScript.id);
                    if (inventory.objectToSaveOnDelivery == null)
                    {
                        inventory.objectToSaveOnDelivery = inventory.dictionary[roboArmScript.id];
                        inventory.dictionary[roboArmScript.id] = null;
                        EventAggregator.putObjectEvent.Publish();
                        roboArmScript.isArmEmpty = true;
                        Debug.Log("Кладём объект в доставщика");
                    }
                }
                else if (inventory.dictionary[roboArmScript.id] != null &&
                         !hit.collider.gameObject.CompareTag("DeliveryBot") &&
                         interact.CanBePutOn)
                {
                
                    RaycastHit hitCheck;
                    Vector3 origin = hit.collider.bounds.center + new Vector3(0,hit.collider.bounds.extents.y/2,0); 
                    Ray ray1 = new Ray(origin, Vector3.up);
                    //todo не получается получить BoxCollider у NonActive Object
                    //maxDist = hit.collider.bounds.size.y
                        if (Physics.Raycast(ray1, out hitCheck, sizeOfObject.y))
                        {
                            return;
                        }
                    inventory.dictionary[roboArmScript.id].SetActive(true);
                    if (hit.collider.CompareTag("box"))
                    {
                        inventory.dictionary[roboArmScript.id].transform.position = hit.collider.bounds.center + new Vector3(0, inventory.dictionary[roboArmScript.id].gameObject.GetComponent<BoxCollider>().bounds.extents.y, 0);
                    }
                    else
                    {
                        inventory.dictionary[roboArmScript.id].transform.position = hit.point;
                    }
                    inventory.dictionary[roboArmScript.id] = null;
                    EventAggregator.putObjectEvent.Publish();
                    roboArmScript.isArmEmpty = true;
                    Debug.Log("Кладём вещь на сцену");
                }
            }
        }
    }
}