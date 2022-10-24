using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    [SerializeField] GameObject destination;
    [SerializeField] float teleportationTime = 1f;

    GameObject device;
    [HideInInspector] public bool isTeleportedTo = false;
    bool startedTeleporting = false;


    void OnTriggerStay(Collider other)
    {
        if (other.GetType() != typeof(CharacterController)) return;
        if (isTeleportedTo || startedTeleporting) return;
        device = GameInfo.currentDevice;
        if (device.tag != "DeliveryBot") return; //can add ui or something
        if (GetComponent<BoxCollider>().bounds.Contains(other.bounds.min) &&
            GetComponent<BoxCollider>().bounds.Contains(other.bounds.max))
        {
            Debug.Log("Lift entered");
            EventAggregator.endMoving.Publish(device);
            StartCoroutine(Teleport());
            EventAggregator.liftMovingEvent.Publish();
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (isTeleportedTo) return;
        Debug.Log("Lift entered");
        device = GameInfo.currentDevice;
        StartCoroutine(Teleport());
    }*/

    private void OnTriggerExit(Collider other)
    {
        isTeleportedTo = false;
        Debug.Log("Lift exit");
    }

    IEnumerator Teleport()
    {
        startedTeleporting = true;
        Vector3 destPosition = destination.transform.position;
        //destPosition.y += device.transform.localScale.y/4; //half of the deliveryBot high
        device.GetComponent<DeviceInfo>().isActive = false;
        yield return new WaitForSeconds(teleportationTime);

        destination.GetComponentInChildren<Lift>().isTeleportedTo = true;
        device.GetComponent<CharacterController>().enabled = false;
        device.transform.position = destPosition;
        device.transform.eulerAngles = new Vector3(0, destination.transform.eulerAngles.y, 0);
        device.transform.eulerAngles += new Vector3(0, 180, 0);
        device.GetComponent<DeviceInfo>().isActive = true;
        device.GetComponent<CharacterController>().enabled = true;
        startedTeleporting = false;
    }
}
