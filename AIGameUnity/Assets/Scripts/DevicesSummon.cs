using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevicesSummon : MonoBehaviour
{
    [SerializeField] GameObject cockroachSummonPlace;
    [SerializeField] GameObject deliverySummonPlace;

    [SerializeField] Sprite activeSprite;
    [SerializeField] Sprite inactiveSprite;

    [SerializeField] Button deliveryButton;
    [SerializeField] Button cockroachButton;

    [SerializeField] DeviceInfo cockroach = null;
    [SerializeField] DeviceInfo delivery = null;

    private void OnEnable()
    {
        if (cockroach == null) FindCockroach();
        if (cockroachButton != null && cockroach.isBlocked == false)
        {
            cockroachButton.GetComponent<Image>().sprite = activeSprite;
            cockroachButton.enabled = true;
        }
        else if (cockroachButton != null)
        {
            cockroachButton.GetComponent<Image>().sprite = inactiveSprite;
            cockroachButton.enabled = false;
        }

        if (delivery == null) FindDelivery();
        if (deliveryButton != null && delivery.isBlocked == false)
        {
            deliveryButton.GetComponent<Image>().sprite = activeSprite;
            deliveryButton.enabled = true;
        }
        else if (deliveryButton != null)
        {
            deliveryButton.GetComponent<Image>().sprite = inactiveSprite;
            deliveryButton.enabled = false;
        }

    }

    public void SummonCockroach()
    {
        StopAllCoroutines();
        //get cockroach device
        if (cockroach == null) FindCockroach();
        if (cockroach == null) return;
        Vector3 sizeVec = cockroach.gameObject.GetComponent<Collider>().bounds.size;
        //get destination position
        Debug.Log(cockroachSummonPlace.transform.position);
        Debug.Log(sizeVec);
        Vector3 destPosition = cockroachSummonPlace.transform.position;
        destPosition.y += sizeVec.y / 2;
        Debug.Log(destPosition);

        //move device
        cockroach.gameObject.transform.position = destPosition;
        cockroach.gameObject.transform.localEulerAngles = Vector3.zero;
        EventAggregator.PanelClosed.Publish();

        //wtf why is it here??
        //EventAggregator.puzzleEnded.Publish(PuzzleEnd.Algorithm);
    }

    void FindCockroach()
    {
        foreach (DeviceInfo device in GameInfo.devices)
            if (device.gameObject.tag == "Bug")
            {
                cockroach = device;
                break;
            }
    }

    public void SummonDelivery()
    {
        //get cockroach device
        if (delivery == null) FindDelivery();
        if (delivery == null) return;

        //get destination position
        Vector3 sizeVec = delivery.gameObject.GetComponent<Collider>().bounds.size;
        Vector3 destPosition = deliverySummonPlace.transform.position;
        destPosition.y += sizeVec.y / 2;

        //move device
        delivery.gameObject.transform.position = destPosition;
        delivery.gameObject.transform.localEulerAngles = Vector3.zero;
        EventAggregator.PanelClosed.Publish();
    }

    void FindDelivery()
    {
        foreach (DeviceInfo device in GameInfo.devices)
            if (device.gameObject.tag == "DeliveryBot")
            {
                delivery = device;
                break;
            }
    }
}
