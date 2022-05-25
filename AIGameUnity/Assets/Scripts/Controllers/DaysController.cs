using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaysController : MonoBehaviour
{
    [SerializeField] GameObject parentForTempObjects;
    [Header("Properties")]
    public int currentDay = 1;
    [SerializeField] int endDay = 7;
    [SerializeField] float suspitionDecrease;

    List<TempObjects> tempObjects;
    
    private SaveManager saveManager;

    private void Awake()
    {
        if (parentForTempObjects != null)
            tempObjects = new List<TempObjects>(parentForTempObjects.GetComponentsInChildren<TempObjects>(true));
        else
            tempObjects = new List<TempObjects>(FindObjectsOfType<TempObjects>(true));

        tempObjects.Sort((x, y) => x.dayDisappear.CompareTo(y.dayDisappear));

        EventAggregator.changeDay.Subscribe(NextDay);
        saveManager = GetComponent<SaveManager>();
    }

    private void Start()
    {
        ObjectStateChange();
    }

    public void NextDay()
    {
        currentDay++;
        Debug.Log("СЕЙЧАС ДЕНЬ:" + currentDay);
        if (currentDay >= endDay) { EventAggregator.endGame.Publish(Endings.TosterEnding); return; } //throw error or something
        EventAggregator.newDayStarted.Publish(currentDay);
        ObjectStateChange();

        //add comp. power from all devices
        foreach (DeviceInfo device in GameInfo.devices)
        {
            if (!device.isBlocked)
                GameInfo.IncreaseCompPower(device.everyDayPrize);
        }

        //decrease suspition
        GameInfo.DecreaseSuspicion(suspitionDecrease);

        if (saveManager)
            saveManager.Save();
        Debug.Log("СЕЙЧАС ДЕНЬ ПОСЛЕ СОХРАНЕНИЯ:" + currentDay);
        
    }

  public  void ObjectStateChange()
    {
        /*foreach (TempObjects tempObject in tempObjects)
        {
            //turn on objects
            if (currentDay >= tempObject.dayAppear) tempObject.gameObject.SetActive(true);
            //turn off objects
            if (currentDay >= tempObject.dayDisappear)
            {
                tempObject.gameObject.SetActive(false);
                //tempObjects.Remove(tempObject);
            }
        }*/
        foreach (TempObjects tempObject in tempObjects)
        {
            if (currentDay >= tempObject.dayAppear && currentDay < tempObject.dayDisappear) tempObject.gameObject.SetActive(true);
            else tempObject.gameObject.SetActive(false);
        }
    }
}
