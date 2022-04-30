using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DeviceList : MonoBehaviour
{
    [SerializeField] List<DeviceInfo> deviceInfos;
    [SerializeField] Color currentColor = Color.green;
    Button[] buttons;
    Button currentActive;

    private void Start()
    {
        if (deviceInfos == null || deviceInfos.Count == 0) deviceInfos = new List<DeviceInfo>(GameInfo.devices);
        foreach (DeviceInfo device in deviceInfos)
            if (string.IsNullOrEmpty(device.nameToDisplay)) device.nameToDisplay = device.name;

        deviceInfos = deviceInfos.OrderBy(x => x.isBlocked).ToList();

        buttons = GetComponentsInChildren<Button>();

        for (int i = 0; i < buttons.Length; i++)
        {
            int x = i;
            if (deviceInfos.Count > i)
            {
                buttons[x].GetComponentInChildren<Text>().text = deviceInfos[x].nameToDisplay;
                if (deviceInfos[x].isBlocked || !deviceInfos[i].gameObject.activeSelf) { buttons[x].interactable = false; }
                else if (deviceInfos[x].name == GameInfo.currentDevice.name) { buttons[x].GetComponent<Image>().color = currentColor; currentActive = buttons[x]; }
                buttons[x].onClick.AddListener(delegate { SwitchTo(deviceInfos[x]); });
            }
            else
            {
                //Destroy(buttons[x].gameObject);
                buttons[x].gameObject.SetActive(false);
            }
        }

        EventAggregator.deviceBought.Subscribe(onDeviceChanges);
        EventAggregator.getObjectEvent.Subscribe(onDeviceChanges);
        EventAggregator.putObjectEvent.Subscribe(onDeviceChanges);
        EventAggregator.DeviceSwitched.Subscribe(OnDeviceSwitch);
    }

    void SwitchTo(DeviceInfo device)
    {
        EventAggregator.PanelClosed.Publish();
        EventAggregator.DeviceSwitched.Publish(device.gameObject);
    }

    void onDeviceChanges()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].gameObject.activeSelf && deviceInfos[i].isBlocked == false && deviceInfos[i].gameObject.activeSelf)
                buttons[i].interactable = true;
            else buttons[i].interactable = false;
        }
    }

    void OnDeviceSwitch(GameObject device)
    {
        foreach (Button button in buttons)
            if (button.GetComponentInChildren<Text>().text == device.GetComponent<DeviceInfo>().nameToDisplay)
            {
                button.GetComponent<Image>().color = currentColor;
                currentActive.GetComponent<Image>().color = Color.white;
                currentActive = button;
            }
    }
}
