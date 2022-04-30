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
          //  Debug.Log(roboArmScript.id + "?");
        //    Debug.Log("Object: " + inventory.dictionary[roboArmScript.id]);
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

                //проверка дистанции
                /*if (roboArmScript.maxInteractDistance < hit.distance)
                {
                    //todo Ссылка на канвас, а не на ребенка UPD
                    //todo Сделать коррутину для показа текста/сделать такою же логику, как и для взаимодействия с UI элементами, тех.документацию бы дописать, мда
                    //  obj.GetComponentInChildren<Text>().enabled = true;
                    // Debug.Log(hit.distance);
                    return;
                }
                else
                {
                    //  obj.GetComponentInChildren<Text>().enabled = false;
                    //  Debug.Log(hit.distance);
                }*/

                if (inventory.dictionary[roboArmScript.id] == null &&
                    !hit.collider.gameObject.CompareTag("DeliveryBot") &&
                    interact.IsPortable)
                {
                    inventory.dictionary[roboArmScript.id] = hit.collider.gameObject;
                    inventory.dictionary[roboArmScript.id].SetActive(false);
                    //публикация взятия объекта для задания
                    EventAggregator.getObjectEvent.Publish();
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
                        Debug.Log("Кладём объект в доставщика");
                    }
                }
                else if (inventory.dictionary[roboArmScript.id] != null &&
                         !hit.collider.gameObject.CompareTag("DeliveryBot") &&
                         interact.CanBePutOn)
                {
                    //todo тест места для спавна
                    inventory.dictionary[roboArmScript.id].SetActive(true);
                    inventory.dictionary[roboArmScript.id].transform.position = hit.point;
                    inventory.dictionary[roboArmScript.id] = null;
                    EventAggregator.putObjectEvent.Publish();
                    Debug.Log("Кладём вещь на сцену");
                }
            }
        }
    }
}