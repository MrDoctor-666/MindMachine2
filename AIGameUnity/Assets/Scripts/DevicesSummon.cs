using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevicesSummon : MonoBehaviour
{
    [SerializeField] GameObject cockroachSummonPlace;
    [SerializeField] GameObject deliverySummonPlace;

   public void SummonCockroach()
    {
        StopAllCoroutines();
        DeviceInfo cockroach = null;
        //get cockroach device
        foreach (DeviceInfo device in GameInfo.devices)
            if (device.gameObject.tag == "Bug" && device.isBlocked == false)
            {
                cockroach = device;
                break;
            }
        if (cockroach == null) return;
        cockroach.GetComponent<Rigidbody>().isKinematic = false;
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
        StartCoroutine(Land(cockroach));
        EventAggregator.PanelClosed.Publish();

        EventAggregator.puzzleEnded.Publish(PuzzleEnd.Algorithm);
    }

    IEnumerator Land(DeviceInfo cockroach)
    {
        yield return new WaitForSeconds(1);
        cockroach.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void SummonDelivery()
    {
        DeviceInfo delivery = null;
        //get cockroach device
        foreach (DeviceInfo device in GameInfo.devices)
            if (device.gameObject.tag == "DeliveryBot" && device.isBlocked == false)
            {
                delivery = device;
                break;
            }
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
}
