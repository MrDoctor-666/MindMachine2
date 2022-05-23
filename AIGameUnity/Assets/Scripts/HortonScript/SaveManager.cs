using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public class Configuration
{
    public DeviceInfoToSave[] deviceInfoToSave;
    public MoveableObject[] moveableObjects;
}

public class MoveableObject
{
    public float x, y, z, progress;
    public bool isActive;
    public string objectNameInInventory;
}
public class DeviceInfoToSave
{
    public bool isActive, isBlocked;
}

public class SaveManager : MonoBehaviour
{
     private DaysController daysController;
     private TaskManager taskManager;
     private Inventory inventory;
     private PuzzleController puzzleController;
    [SerializeField] private GameObject[] moveableGameObjects;

    private void Awake()
    {
        daysController = GameObject.Find("GameContr").GetComponent<DaysController>();
        taskManager = GameObject.Find("GameContr").GetComponent<TaskManager>();
        inventory = GameObject.Find("GameContr").GetComponent<Inventory>();
        puzzleController = GameObject.Find("GameContr").GetComponent<PuzzleController>();
    }

    private void Start()
    {
        //PlayerPrefs.DeleteAll();
       Load();
     
       print("DIR:" +Directory.GetCurrentDirectory());
    }

    public void Save()
    {
        SaveConfiguration();
        PlayerPrefs.SetString("currDevice", GameInfo.currentDevice.name);
        PlayerPrefs.SetInt("CurrentDay", daysController.currentDay);
        PlayerPrefs.SetFloat("CurrentSuspicion", GameInfo.suspicion);
        PlayerPrefs.SetFloat("CurrentComputingPower", GameInfo.computingPower);
        PlayerPrefs.SetInt("CurPuzzleLevel", puzzleController.curPuzzleLevel);
        SaveTasks(taskManager.taskList, "tasks");
         SaveTasks(taskManager.allTasksList, "taskBank");
        PlayerPrefs.Save();
    }

    public void SaveConfiguration()
    {
        XmlSerializer xml = new XmlSerializer(typeof(Configuration));
        Configuration configuration = new Configuration();

        configuration.moveableObjects = new MoveableObject[moveableGameObjects.Length];
        for (int i = 0; i < moveableGameObjects.Length; i++)
        {
            configuration.moveableObjects[i] = new MoveableObject();
            if (moveableGameObjects[i].GetComponent<Roboarm>())
            {
                Roboarm roboarm = moveableGameObjects[i].GetComponent<Roboarm>();
                configuration.moveableObjects[i].progress = roboarm.GetComponent<Roboarm>().progress;
                configuration.moveableObjects[i].isActive = moveableGameObjects[i].activeSelf;
                if (inventory.dictionary[roboarm.id] != null)
                {
                    configuration.moveableObjects[i].objectNameInInventory = inventory.dictionary[roboarm.id].name;
                }
            }
            else
            {
                configuration.moveableObjects[i].x = moveableGameObjects[i].transform.position.x;
                configuration.moveableObjects[i].y = moveableGameObjects[i].transform.position.y;
                configuration.moveableObjects[i].z = moveableGameObjects[i].transform.position.z;
                if (moveableGameObjects[i].GetComponent<ThoughtsTrigger>())
                {
                    configuration.moveableObjects[i].isActive =
                        moveableGameObjects[i].GetComponent<BoxCollider>().enabled;
                }
                else
                {
                    configuration.moveableObjects[i].isActive = moveableGameObjects[i].activeSelf;
                }

                if (moveableGameObjects[i].GetComponent<DeliveryMove>() && inventory.objectToSaveOnDelivery != null)
                {
                    configuration.moveableObjects[i].objectNameInInventory = inventory.objectToSaveOnDelivery.name;
                }
            }
        }

        //save deviceInfo 
        configuration.deviceInfoToSave = new DeviceInfoToSave[GameInfo.devices.Length];
        for (int i = 0; i < GameInfo.devices.Length; i++)
        {
            configuration.deviceInfoToSave[i] = new DeviceInfoToSave();
            configuration.deviceInfoToSave[i].isActive = GameInfo.devices[i].isActive;
            configuration.deviceInfoToSave[i].isBlocked = GameInfo.devices[i].isBlocked;
        }
        
        //save ClickOnDevice
        using (var stream = new FileStream(Application.dataPath +"/"+ "Save.xml", FileMode.Create, FileAccess.Write))
        {
            xml.Serialize(stream, configuration);
        }

        print("Сериализовали конфигурационный файл.");
    }
     public void Load()
    {
        LoadConfiguration();
        EventAggregator.DeviceSwitched.Publish(GameObject.Find(PlayerPrefs.GetString("currDevice")));
        EventAggregator.newDayStarted.Publish(daysController.currentDay =
            PlayerPrefs.HasKey("CurrentDay") ? PlayerPrefs.GetInt("CurrentDay") : 1);
        GameInfo.suspicion = PlayerPrefs.GetFloat("CurrentSuspicion");
        GameInfo.computingPower = PlayerPrefs.GetFloat("CurrentComputingPower");
        puzzleController.curPuzzleLevel = PlayerPrefs.GetInt("CurPuzzleLevel");
        LoadTasks(taskManager.taskList, "tasks");
        LoadTasks(taskManager.allTasksList, "taskBank");
    }

    private void LoadConfiguration()
    {
        XmlSerializer xml = new XmlSerializer(typeof(Configuration));
        Configuration configuration = new Configuration();

        using (var stream = new FileStream(Application.dataPath +"/"+ "Save.xml", FileMode.Open, FileAccess.Read))
        {
            configuration = xml.Deserialize(stream) as Configuration;
        }

        if (configuration != null)
        {
            for (int i = 0; i < moveableGameObjects.Length; i++)
            {
                if (moveableGameObjects[i].GetComponent<Roboarm>())
                {
                    Roboarm roboarm = moveableGameObjects[i].GetComponent<Roboarm>();
                    roboarm.progress = configuration.moveableObjects[i].progress;
                    moveableGameObjects[i].SetActive(configuration.moveableObjects[i].isActive);
                    
                    if (configuration.moveableObjects[i].objectNameInInventory != null)
                    {
                        inventory.dictionary[roboarm.id] =
                            GameObject.Find(configuration.moveableObjects[i].objectNameInInventory);
                    }
                    
                }
                else
                {
                    moveableGameObjects[i].transform.position = new Vector3(configuration.moveableObjects[i].x,
                        configuration.moveableObjects[i].y, configuration.moveableObjects[i].z);
                    
                    if (moveableGameObjects[i].GetComponent<ThoughtsTrigger>())
                    {
                        
                        moveableGameObjects[i].GetComponent<BoxCollider>().enabled =
                            configuration.moveableObjects[i].isActive;
                    }
                    else
                    {
                        moveableGameObjects[i].SetActive(configuration.moveableObjects[i].isActive);
                    }
                    
                    if (moveableGameObjects[i].GetComponent<DeliveryMove>() &&
                        configuration.moveableObjects[i].objectNameInInventory != null)
                    {
                        inventory.objectToSaveOnDelivery =
                            GameObject.Find(configuration.moveableObjects[i].objectNameInInventory);
                    }
                }
            }

            //load DevicesInfo
            for (int i = 0; i < GameInfo.devices.Length; i++)
            {
                GameInfo.devices[i].isActive = configuration.deviceInfoToSave[i].isActive;
                GameInfo.devices[i].isBlocked = configuration.deviceInfoToSave[i].isBlocked;
            }
            print("Загрузили данные из конфигурационного файла");
        }
        else
        {
            print("CONFIGURATION IS NULL!!!");
        }
    }
    private void SaveTasks(List<TaskSO> taskList, string key)
    {
        for (int i = 0; i < taskList.Count; i++)
        {
            PlayerPrefs.SetString(key + i, JsonUtility.ToJson(taskList[i], true));
            PlayerPrefs.SetString(key + "name" + i, taskList[i].name);
            print("Имя задания "+PlayerPrefs.GetString( key + "name" + i));
            print("Задание "+PlayerPrefs.GetString(key + i));
        }
    }

    private void LoadTasks()
    {
        EventAggregator.newDayStarted.Publish(daysController.currentDay =
            PlayerPrefs.HasKey("CurrentDay") ? PlayerPrefs.GetInt("CurrentDay") : 1);
        daysController.ObjectStateChange();

        if (PlayerPrefs.HasKey("tasks"))
            taskManager.taskList = JsonUtility.FromJson<List<TaskSO>>(PlayerPrefs.GetString("tasks"));
    }

    private void LoadTasks(List<TaskSO> taskList, string key)
    {  
        taskList.Clear();
        TaskSO taskSo;
        int i = 0;
        while (PlayerPrefs.HasKey(key+i))
        {
            taskSo = ScriptableObject.CreateInstance<TaskSO>();
            taskSo.name = PlayerPrefs.GetString(key + "name" + i);
            taskList.Add(taskSo);
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key+i), taskList[i]);
            i++;
        }
        taskManager.ChangeMissionWaypoint(taskList[0]);
    }
}