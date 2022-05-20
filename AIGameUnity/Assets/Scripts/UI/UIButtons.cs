using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtons : MonoBehaviour
{
    public void Close()
    {
        EventAggregator.clickButtonUI.Publish();
        gameObject.SetActive(false);
        EventAggregator.PanelClosed.Publish();
    }

    public void YesToButDevice()
    {
        EventAggregator.clickButtonUI.Publish();

        //actually buy it
        EventAggregator.deviceBought.Publish();
        EventAggregator.PanelClosed.Publish();
        gameObject.SetActive(false);
    }
    public void YesToSwitchDay()
    {
        EventAggregator.clickButtonUI.Publish();

        //Turn to next day
        EventAggregator.PanelClosed.Publish();
        EventAggregator.changeDay.Publish();
        gameObject.SetActive(false);
    }

    public void SwitchInTab(GameObject switchTo)
    {
        foreach(Transform child in transform)
            if (child.GetComponent<Button>() == null) child.gameObject.SetActive(false);
        switchTo.SetActive(true);
    }
}
