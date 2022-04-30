using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  [SerializeField]  private Camera curCamera;
    private void Awake()
    {
        //curCamera = Camera.main;
        //curCamera.enabled = true;
        EventAggregator.DeviceSwitched.Subscribe(OnDeviceSwitch);
    }

    private void Start()
    {
        curCamera = GameInfo.currentDevice.GetComponentInChildren<Camera>();

    }

    private void OnDeviceSwitch(GameObject device)
    {
        Camera cam = curCamera;
        curCamera = device.GetComponentInChildren<Camera>();
        if (cam != null) cam.enabled = false;
        curCamera.enabled = true;
    }
}
