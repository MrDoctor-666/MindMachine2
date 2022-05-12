using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DeviceList : MonoBehaviour
{
    [Header("For image")]
    [SerializeField] Sprite activeSprite;
    [SerializeField] Sprite disabledSprite;
    [SerializeField] Font currentDeviceFont;
    [SerializeField] Color currentColor = Color.green;
    Font defaultFont;

    [Header("Manual settings")]
    [SerializeField] List<DeviceInfo> deviceInfos;

    Button[] buttons;
    Button currentActive;

    private void Start()
    {
        if (deviceInfos == null || deviceInfos.Count == 0) deviceInfos = new List<DeviceInfo>(GameInfo.devices);
        foreach (DeviceInfo device in deviceInfos)
            if (string.IsNullOrEmpty(device.nameToDisplay)) device.nameToDisplay = device.name;

        deviceInfos = deviceInfos.OrderBy(x => x.isBlocked).ToList();

        buttons = GetComponentsInChildren<Button>();
        defaultFont = buttons[0].GetComponentInChildren<Text>().font;
        if (activeSprite == null) activeSprite = buttons[0].GetComponent<Image>().sprite;

        for (int i = 0; i < buttons.Length; i++)
        {
            int x = i;
            if (deviceInfos.Count > i)
            {
                buttons[x].GetComponent<Image>().sprite = activeSprite;
                buttons[x].GetComponentInChildren<Text>().text = deviceInfos[x].nameToDisplay;
                if (deviceInfos[x].isBlocked || !deviceInfos[i].gameObject.activeSelf) { 
                    buttons[x].interactable = false;
                    buttons[x].GetComponent<Image>().sprite = disabledSprite;
                }
                else if (deviceInfos[x].name == GameInfo.currentDevice.name) { 
                    buttons[x].GetComponent<Image>().color = currentColor;
                    buttons[x].GetComponentInChildren<Text>().font = currentDeviceFont;
                    currentActive = buttons[x]; 
                }
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
            {
                buttons[i].interactable = true;
                //change sprite
                buttons[i].GetComponent<Image>().sprite = activeSprite;
            }
            else
            {
                buttons[i].interactable = false;
                //change sprite
                buttons[i].GetComponent<Image>().sprite = disabledSprite;
            }
        }
    }

    void OnDeviceSwitch(GameObject device)
    {
        foreach (Button button in buttons)
            if (button.GetComponentInChildren<Text>().text == device.GetComponent<DeviceInfo>().nameToDisplay)
            {
                currentActive.GetComponent<Image>().color = Color.white;
                button.GetComponent<Image>().color = currentColor;
                currentActive.GetComponentInChildren<Text>().font = defaultFont;
                button.GetComponentInChildren<Text>().font = currentDeviceFont;
                currentActive = button;
            }
    }
}
