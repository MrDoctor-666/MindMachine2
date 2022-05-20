using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DeviceController : MonoBehaviour
{
    [SerializeField] public GameObject currentDevice;
    private GameObject deviceBuy = null;
    
    
    private void Awake()
    {
        GameInfo.currentDevice = currentDevice;
        Debug.Log("AWAKE" + currentDevice); 

        EventAggregator.DeviceSwitched.Subscribe(OnDeviceSwitch);

        EventAggregator.DeviceBuyError.Subscribe(OnStopWorking);
        EventAggregator.DeviceBuyTried.Subscribe(OnDeviceBuyTry);
        EventAggregator.deviceBought.Subscribe(OnDeviceBought);

        EventAggregator.PanelOpened.Subscribe(OnStopWorking);
        EventAggregator.DialogueStarted.Subscribe(OnStopWorking);
        EventAggregator.PanelClosed.Subscribe(OnResumeWorking);

        EventAggregator.puzzleStarted.Subscribe(OnStopWorking);
        EventAggregator.puzzleEnded.Subscribe(OnPuzzleEnded);
    }

    private void Start()
    {
        GameInfo.devices = FindObjectsOfType<DeviceInfo>(true);
        foreach (DeviceInfo device in GameInfo.devices)
        {
            Debug.Log(device.name);
            device.isActive = false;
            device.GetComponentInChildren<Camera>().enabled = false;
            device.GetComponentInChildren<AudioListener>().enabled = false;
        }
        //EventAggregator.DeviceSwitched.Publish(GameInfo.currentDevice);
        currentDevice.GetComponent<DeviceInfo>().isActive = true;
        currentDevice.GetComponentInChildren<AudioListener>().enabled = true;
        currentDevice.GetComponentInChildren<Camera>().enabled = true;
        if (currentDevice.tag == "Bug") currentDevice.GetComponent<Rigidbody>().freezeRotation = true;
        EventAggregator.DeviceSwitched.Publish(GameInfo.currentDevice);
    }

    public void OnDeviceSwitch(GameObject device)
    {
        Debug.Log("On device switch to " + device);
        Cursor.lockState = CursorLockMode.None;
        InteractionData.Reset();
        EventAggregator.IntercationAreaExited.Publish(currentDevice);
        currentDevice.GetComponent<DeviceInfo>().isActive = false;
        currentDevice.GetComponentInChildren<AudioListener>().enabled = false;
        device.GetComponent<DeviceInfo>().isActive = true;
        device.GetComponentInChildren<AudioListener>().enabled = true;
        if (currentDevice.tag == "Bug") currentDevice.GetComponent<Rigidbody>().freezeRotation = false;
        if (device.tag == "Bug") device.GetComponent<Rigidbody>().freezeRotation = true;
        //canvases
        if (device.GetComponentInChildren<Canvas>(true)) device.GetComponentInChildren<Canvas>(true).gameObject.SetActive(true);
        if (currentDevice.GetComponentInChildren<Canvas>(true)) currentDevice.GetComponentInChildren<Canvas>(true).gameObject.SetActive(false);

        currentDevice = device;
        GameInfo.currentDevice = currentDevice;
    }

    public void OnStopWorking(GameObject gameObject)
    {
        Debug.Log("current device   " + currentDevice);
        Debug.Log("current device in gameinfo" + GameInfo.currentDevice);
        //currentDevice.GetComponent<DeviceInfo>().isActive = false;
        GameInfo.currentDevice.GetComponent<DeviceInfo>().isActive = false;
        EventAggregator.endMoving.Publish(GameInfo.currentDevice);
        //EventAggregator.endMoving.Publish(currentDevice);
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnResumeWorking()
    {
        currentDevice.GetComponent<DeviceInfo>().isActive = true;
        currentDevice.GetComponentInChildren<AudioListener>().enabled = true;
        //if (currentDevice.name == "BugCocroach") Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnDeviceBuyTry(GameObject device)
    {
        OnStopWorking(device);
        deviceBuy = device;
    }

    public void OnDeviceBought()
    {
        string messageIfUnlocked = deviceBuy.GetComponent<OnClickDevice>().messageIfUnlocked;
        currentDevice.GetComponentInChildren<AudioListener>().enabled = false;
        EventAggregator.puzzleStarted.Publish(deviceBuy);
        deviceBuy.GetComponent<DeviceInfo>().isBlocked = false;
        deviceBuy.GetComponent<OnClickDevice>().SwitchTooltipMessage(messageIfUnlocked);
        GameInfo.DecreaseCompPower(deviceBuy.GetComponent<DeviceInfo>().cost);
        Debug.Log(GameInfo.computingPower);
        deviceBuy = null;
    }

    private void OnPuzzleEnded(PuzzleEnd puzEnd)
    {
        OnResumeWorking();
    }
}
