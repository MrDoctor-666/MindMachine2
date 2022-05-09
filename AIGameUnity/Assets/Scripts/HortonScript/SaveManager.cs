using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
//using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;


[Serializable]
public class Configuration
{
    public Beatle beatleSave;
    public DeliveryBot deliverySave;
    public RoboarmSave roboarmSave;
    public DeviceInfoToSave[] deviceInfoToSave;
    public TosterThoughts[] tosterThoughts;
    public Task[] taskList, allTasksInBank;
    public InventorySave inventoryDelivery;
    public InventorySave inventoryRoboarm1;
    public InventorySave inventoryRoboarm2;
    public InventorySave inventoryRoboarm3;
}


public class Beatle
{
    public float x, y, z;
    public bool isActive, isBlocked, isActiveOnScene;
}

public class DeliveryBot
{
    public float x, y, z;
}

public class RoboarmSave
{
    public float x, y, z;
}

public class InventorySave
{
    public string objectInInventory;
}

public class TosterThoughts
{
    public bool isActive;
}

public class Task
{
    public string name;
    public string taskInfo; 
    public int firstAvailableDay;
    public int lastAvailableDay;
    public int priority;
    public string diaryInfo;
}

public class DeviceInfoToSave
{
    public bool isActive, isBlocked;
}

public class SaveManager: MonoBehaviour
{
    [SerializeField] private GameObject deliveryBot;
    [SerializeField] private GameObject beatle;
    [SerializeField] private DaysController daysController;
    [SerializeField] private TaskManager taskManager;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Roboarm roboArmScript;
    [SerializeField] private GameObject[] tosterThoughtsGO;
    [SerializeField] private DialogueTrigger[] dialogues;

    private void Start()
    {
        PlayerPrefs.DeleteAll();
      // Load();
    }

    public void Save()
    {
        SaveConfiguration();
        
        //LoadConfiguration();
    //    SaveDeviceInfo(GameInfo.devices);
    //    SaveBeatle();
     //   LoadDeviceInfo(GameInfo.devices);
        //SaveTasks();
        SaveTasks(taskManager.taskList, "task");
        SaveTasks(taskManager.allTasksList, "taskBank");
     //   SaveDevicesTransforms();
      // SaveArray(GameInfo.devices, "activeDevices");
     //   SaveThoughts(tosterThoughts, "tosterThoughts");
     /*   SaveObjectInfo(inventory.dictionary[1], "roboarm1");
        SaveObjectInfo(inventory.dictionary[2], "roboarm2");
        SaveObjectInfo(inventory.objectToSaveOnDelivery, "deliveryBotInventory");
        SaveObjectInfo(GameInfo.currentDevice, "currDevice");
        */
     //  SaveDialogues(); // check this

        PlayerPrefs.Save();

    }

    public void SaveConfiguration()
    {
        XmlSerializer xml = new XmlSerializer(typeof(Configuration));
        Configuration configuration = new Configuration();
        
        configuration.beatleSave = new Beatle();
        //save transform beatle
        configuration.beatleSave.x = beatle.transform.position.x;
        configuration.beatleSave.y = beatle.transform.position.y;
        configuration.beatleSave.z = beatle.transform.position.z;
        //save transform delivery
        configuration.deliverySave = new DeliveryBot();
        configuration.deliverySave.x = deliveryBot.transform.position.x;
        configuration.deliverySave.y = deliveryBot.transform.position.y;
        configuration.deliverySave.z = deliveryBot.transform.position.z;
        //save roboarm transform
        var pos = roboArmScript.gameObject.transform.root.transform.position;
        configuration.roboarmSave = new RoboarmSave();
        configuration.roboarmSave.x = pos.x;
        configuration.roboarmSave.y = pos.y;
        configuration.roboarmSave.z = pos.z;
        
        configuration.deviceInfoToSave =  new DeviceInfoToSave[GameInfo.devices.Length];
        //save deviceInfo 
        for (int i = 0; i < GameInfo.devices.Length; i++)
        {
            configuration.deviceInfoToSave[i] = new DeviceInfoToSave();
            configuration.deviceInfoToSave[i].isActive = GameInfo.devices[i].isActive;
            configuration.deviceInfoToSave[i].isBlocked = GameInfo.devices[i].isBlocked;
        }

        configuration.tosterThoughts = new TosterThoughts[tosterThoughtsGO.Length];
        //save status of Thoughts 
        for (int i = 0; i < tosterThoughtsGO.Length; i++)
        {
            configuration.tosterThoughts[i] = new TosterThoughts();
            configuration.tosterThoughts[i].isActive = tosterThoughtsGO[i].activeSelf;
        }
        // configuration.taskList = new Task[taskManager.taskList.Count];
        // //save taskList
        // for (int i = 0; i < taskManager.taskList.Count; i++)
        // {
        //     configuration.taskList[i] = new Task();
        //     // configuration.taskList[i].taskInfo = taskManager.taskList[i].taskInfo;
        //     // configuration.taskList[i].firstAvailableDay = taskManager.taskList[i].firstAvailableDay;
        //     // configuration.taskList[i].lastAvailableDay = taskManager.taskList[i].lastAvailableDay;
        //     // configuration.taskList[i].priority = taskManager.taskList[i].priority;
        //     // configuration.taskList[i].diaryInfo = taskManager.taskList[i].diaryInfo;
        //     configuration.taskList[i].name = taskManager.taskList[i].name;
        // }
        //
        // configuration.allTasksInBank = new Task[taskManager.allTasksList.Count];
        //
        // //save tasks in bank;
        // for (int i = 0; i < taskManager.allTasksList.Count; i++)
        // {
        //     configuration.allTasksInBank[i] = new Task();
        //     configuration.allTasksInBank[i].taskInfo = taskManager.allTasksList[i].taskInfo;
        //     configuration.allTasksInBank[i].firstAvailableDay = taskManager.allTasksList[i].firstAvailableDay;
        //     configuration.allTasksInBank[i].lastAvailableDay = taskManager.allTasksList[i].lastAvailableDay;
        //     configuration.allTasksInBank[i].priority = taskManager.allTasksList[i].priority;
        //     configuration.allTasksInBank[i].diaryInfo = taskManager.allTasksList[i].diaryInfo;
        // }

        configuration.inventoryDelivery = new InventorySave();
        //save inventory in delivery 
        if (inventory.objectToSaveOnDelivery.Equals(null))
        {
            configuration.inventoryDelivery.objectInInventory = null;
        }
        else 
        {
            configuration.inventoryDelivery.objectInInventory = inventory.gameObject.name;
        }
        //todo redo for array of arms
        //save inventory of roboarms
        configuration.inventoryRoboarm1 = new InventorySave();
        // if (inventory.dictionary[1].Equals(null))
        // {
        //     configuration.inventoryRoboarm1.objectInInventory = null;
        // }
        // else
        // {
        //     configuration.inventoryRoboarm1.objectInInventory = inventory.dictionary[1].name;
        // }
        // configuration.inventoryRoboarm2 = new InventorySave();
        // if (inventory.dictionary[2].Equals(null))
        // {
        //     configuration.inventoryRoboarm2.objectInInventory = null;
        // }
        // else
        // {
        //     configuration.inventoryRoboarm2.objectInInventory = inventory.dictionary[2].name;
        // }
        //
        // configuration.inventoryRoboarm3 = new InventorySave();
        // if (inventory.dictionary[3].Equals(null))
        // {
        //     configuration.inventoryRoboarm3.objectInInventory = null;
        // }
        // else
        // {
        //     configuration.inventoryRoboarm3.objectInInventory = inventory.dictionary[3].name;
        // }

        using (var stream = new FileStream("Test.xml", FileMode.Create, FileAccess.Write))
        {
            xml.Serialize(stream, configuration);
        }
        print("Сериализовали конфигурационный файл.");
    }
    private void SaveTasks()
    {
        PlayerPrefs.SetInt("CurrentDay", daysController.currentDay);
        //не дает сериализовать весь лист
        PlayerPrefs.SetString("tasks", JsonUtility.ToJson(taskManager.taskList, true));
    }
    
    private void SaveDevicesTransforms()
    {
        Transform T;
        
        T = deliveryBot.transform;
        PlayerPrefs.SetString("DeliveryPosition", $"{T.position.x} {T.position.y} {T.position.z}");
        PlayerPrefs.SetString("DeliveryRotation", $"{T.eulerAngles.x} {T.eulerAngles.y} {T.eulerAngles.z}");

        T = roboArmScript.gameObject.transform.root.transform;
        PlayerPrefs.SetString("RoboArmPosition", $"{T.position.x} {T.position.y} {T.position.z}");
        PlayerPrefs.SetFloat("RoboArmProgress", roboArmScript.progress);
   //     PlayerPrefs.SetString("RoboArmSpline", EditorJsonUtility.ToJson(roboArmScript.spline));

        T = beatle.transform;
        PlayerPrefs.SetString("BeatlePosition", $"{T.position.x} {T.position.y} {T.position.z}");
        PlayerPrefs.SetString("BeatleRotation", $"{T.eulerAngles.x} {T.eulerAngles.y} {T.eulerAngles.z}");
    }

 /*   private void SaveObjectInfo<M>(M obj, string key)
    {
        PlayerPrefs.SetString(key, EditorJsonUtility.ToJson(obj));
        print(PlayerPrefs.GetString(key));
    }*/

    //todo возможно не нужна вообще, а просто переделать под Json-ToJson 
    private void SaveArray<M>(M[] obj, string key)
    {
        for (int i = 0; i < obj.Length; i++)
        {
            PlayerPrefs.SetString(key+i, JsonUtility.ToJson(obj[i]));
            print(PlayerPrefs.GetString(key+i));
        }
    }
    private void SaveTasks (List<TaskSO> taskList, string key)
    {
        for (int i = 0; i < taskList.Count; i++)
        {
            PlayerPrefs.SetString(key+i, JsonUtility.ToJson(taskList[i], true));
            print(PlayerPrefs.GetString(key+i));
        }
    }
    private void SaveThoughts(GameObject[] obj, string key)
    {
        for (int i = 0; i < obj.Length; i++)
        {
            if (obj[i].gameObject.activeSelf)
            {
                PlayerPrefs.SetInt(key+i, 1);
            }
            else
            {
                PlayerPrefs.SetInt(key+i, 0);
            }
            print(PlayerPrefs.GetInt(key+i));
        }
    }

    private void SaveDialogues()
    {
        for (int i = 0; i < dialogues.Length; ++i)
            PlayerPrefs.SetInt($"dialogue{i}", dialogues[i].IsInteractable ? 1 : 0);
    }

    public void Load()
    {
        LoadTasks(taskManager.taskList, "task");
        LoadTasks(taskManager.allTasksList, "taskBank");
        //LoadTasks();
       // LoadDevicesTransfrom();
        // LoadRoboArmInventory();
        // LoadDeliveryBotInventory();
        // LoadCurrentDevice();
       // LoadDialoguesState(); // check this
       // LoadArray(GameInfo.devices, "activeDevices");
       // LoadThoughts(tosterThoughts, "tosterThoughts");
    }

    private void LoadConfiguration()
    {
        XmlSerializer xml = new XmlSerializer(typeof(Configuration));
        Configuration configuration = new Configuration();
        
        using (var stream = new FileStream("Test.xml", FileMode.Open, FileAccess.Read))
        {
            configuration = xml.Deserialize(stream) as Configuration;
        }

        if (configuration!=null)
        {
            //load beatle transform
            beatle.transform.position = new Vector3(configuration.beatleSave.x, configuration.beatleSave.y,
                configuration.beatleSave.z);
            //load deliveryBot transform 
            deliveryBot.transform.position = new Vector3(configuration.deliverySave.x, configuration.deliverySave.y,
                configuration.deliverySave.z);
            
            //load roboarm1 transform 
            roboArmScript.gameObject.transform.root.transform.position = new Vector3(configuration.roboarmSave.x,
                configuration.roboarmSave.y, configuration.roboarmSave.z);
            
            //load DevicesInfo
            for (int i = 0; i < GameInfo.devices.Length; i++)
            {
                GameInfo.devices[i].isActive = configuration.deviceInfoToSave[i].isActive;
                GameInfo.devices[i].isBlocked = configuration.deviceInfoToSave[i].isBlocked;
            }
            //load taskList
            // for (int i = 0; i < configuration.taskList.Length; i++)
            // {
            //     // TaskSO taskSo = new TaskSO();
            //     //
            //     // taskSo.taskInfo = configuration.taskList[i].diaryInfo;
            //     // taskSo.firstAvailableDay = configuration.taskList[i].firstAvailableDay;
            //     // taskSo.lastAvailableDay = configuration.taskList[i].lastAvailableDay;
            //     // taskSo.priority = configuration.taskList[i].priority;
            //     // taskSo.diaryInfo = configuration.taskList[i].diaryInfo;
            //     
            //             
            //     //taskManager.taskList.Add();
            // }
            //todo check for null object in inventory of do in active or not 
            //load object from scene to inventory of deliveryBot 
            inventory.objectToSaveOnDelivery = GameObject.Find(configuration.inventoryDelivery.objectInInventory);
            GameObject.Find(configuration.inventoryDelivery.objectInInventory).SetActive(false);
            //load object from scene to inventory of roboarm1
            inventory.dictionary[1] = GameObject.Find(configuration.inventoryRoboarm1.objectInInventory);
            GameObject.Find(configuration.inventoryRoboarm1.objectInInventory).SetActive(false);
            //load object from scene to inventory of roboarm2
            inventory.dictionary[2] = GameObject.Find(configuration.inventoryRoboarm2.objectInInventory);
            GameObject.Find(configuration.inventoryRoboarm2.objectInInventory).SetActive(false);
            //load object from scene to inventory of roboarm3
            inventory.dictionary[3] = GameObject.Find(configuration.inventoryRoboarm3.objectInInventory);
            GameObject.Find(configuration.inventoryRoboarm3.objectInInventory).SetActive(false);
            
            print("Загрузили данные из конфигурационного файла");
        }
        
    }

    private void LoadTasks()
    {
        EventAggregator.newDayStarted.Publish(daysController.currentDay = PlayerPrefs.HasKey("CurrentDay") ? PlayerPrefs.GetInt("CurrentDay") : 1);
        daysController.ObjectStateChange();
        
        if (PlayerPrefs.HasKey("tasks"))
            taskManager.taskList = JsonUtility.FromJson<List<TaskSO>>(PlayerPrefs.GetString("tasks"));
    }

    // private void LoadCurrentDevice()
    // {
    //     if (PlayerPrefs.HasKey("currDevice") && !string.IsNullOrEmpty(PlayerPrefs.GetString("currDevice")))
    //         EventAggregator.DeviceSwitched.Publish(GameInfo.currentDevice = LoadObjectInfo(GameInfo.currentDevice, "currDevice"));
    // }

    private void LoadDevicesTransfrom()
    {
        string[] values;

        try
        {
            values = PlayerPrefs.GetString("DeliveryPosition").Split(' ');
            deliveryBot.transform.position = new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));
            values = PlayerPrefs.GetString("DeliveryRotation").Split(' ');
            deliveryBot.transform.rotation = Quaternion.Euler(new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2])));
        
            roboArmScript.progress = PlayerPrefs.GetFloat("RoboArmProgress");
            values =  PlayerPrefs.GetString("RoboArmPosition").Split(' ');
            roboArmScript.transform.root.transform.position = new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));
            roboArmScript.spline = JsonUtility.FromJson<BezierSpline>(PlayerPrefs.GetString("RoboArmSpline"));

            values = PlayerPrefs.GetString("BeatlePosition").Split(' ');
            beatle.transform.position = new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));
            values = PlayerPrefs.GetString("BeatleRotation").Split(' ');
            beatle.transform.rotation = Quaternion.Euler(new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2])));
        }
        catch { Debug.LogError("SaveManager.LoadDevicesTransfrom(): no data about devices to load"); }
    }

    // private void LoadRoboArmInventory()
    // {
    //     if (!(PlayerPrefs.HasKey("roboarm1") && PlayerPrefs.HasKey("roboarm2")))
    //         return;
    //
    //     if (!string.IsNullOrEmpty(PlayerPrefs.GetString("roboarm1")))
    //     {
    //         inventory.dictionary[1] = new GameObject();
    //         inventory.dictionary[1] = LoadObjectInfo(inventory.dictionary[1], "roboarm1");
    //     }
    //     if (!string.IsNullOrEmpty(PlayerPrefs.GetString("roboarm2")))
    //     {
    //         inventory.dictionary[2] = new GameObject();
    //         inventory.dictionary[2] = LoadObjectInfo(inventory.dictionary[2], "roboarm2");
    //     }
    // }

    // private void LoadDeliveryBotInventory()
    // {
    //     if (PlayerPrefs.HasKey("deliveryBotInventory") && !string.IsNullOrEmpty(PlayerPrefs.GetString("deliveryBotInventory")))
    //     {
    //         inventory.objectToSaveOnDelivery = new GameObject();
    //         inventory.objectToSaveOnDelivery = LoadObjectInfo(inventory.objectToSaveOnDelivery, "deliveryBotInventory");
    //     }
    // }

    private void LoadDialoguesState()
    {
        //if (PlayerPrefs.HasKey("dialogues"))
        //    for (int i = 0; i < dialogues.Length; i++)
        //    {
        //        DialogueTrigger d = JsonConvert.DeserializeObject<DialogueTrigger>(PlayerPrefs.GetString("dialogues" + i));
        //        dialogues[i].IsInteractable = d.IsInteractable;
        //    }

        for (int i = 0; i < dialogues.Length; ++i)
        {
            if (!PlayerPrefs.HasKey($"dialogue{i}"))
                Debug.LogError("SaveManager.LoadDialogueState(): no data about dialogue to load");

            dialogues[i].IsInteractable = PlayerPrefs.GetInt($"dialogue{i}") == 1;
        }
    }

 /*   private GameObject LoadObjectInfo <H>(H obj, string key)
    { 
        EditorJsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), obj);

        return obj as GameObject;
             }
    */
    
    private void LoadArray <H>(H[] obj, string key)
    {
        for (int i = 0; i < obj.Length; i++)
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key+i), obj[i]);
        }

    }
    private void LoadTasks(List<TaskSO> taskList, string key)
    {
        for (int i = 0; i < taskList.Count; i++)
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key+i), taskList[i]);
        }

    }
    private void LoadThoughts(GameObject[] obj, string key)
    {
        for (int i = 0; i < obj.Length; i++)
        {
            if (PlayerPrefs.GetInt(key+i) == 1)
            {
                obj[i].SetActive(true);
            }
            else
            {
                obj[i].SetActive(false);
            }
            print(PlayerPrefs.GetInt(key+i));
        }

    }
}