using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelOfRoboarm : MonoBehaviour
{
    [SerializeField] private GameObject model;
    [SerializeField] private DeviceController deviceController;

    private void Update()
    {
        Roboarm();
    }

    private void Roboarm()
    {
        if (deviceController.currentDevice.GetComponent<Roboarm>())
        {
            model.SetActive(false);
        }
        else 
        {
            model.SetActive(true);
        }
    }

}
