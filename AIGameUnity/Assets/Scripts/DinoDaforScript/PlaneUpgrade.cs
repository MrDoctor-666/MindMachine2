using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneUpgrade : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log("Plane Upgrade activated");
        //todo лучше бы сделать через систему ивентов конечно бы;
        gameObject.transform.root.GetComponent<DeliveryMove>().isWithPlane = true;
    }
}
