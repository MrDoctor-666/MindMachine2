using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using Object = System.Object;
using Newtonsoft.Json;


public class SaveManager: MonoBehaviour
{
    [SerializeField] private GameObject deliveryBot;
    [SerializeField] private GameObject beatle;
    [SerializeField] private DaysController daysController;
    [SerializeField] private TaskManager taskManager;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Roboarm roboArmScript;
    [SerializeField] private GameObject[] tosterThoughts;
    [SerializeField] private DialogueTrigger[] dialogues;

    private void Start()
    {
       PlayerPrefs.DeleteAll();
     // Load();
    }

    public void Save()
    {
        SaveTasks();
        SaveDevicesTransforms();
        SaveArray(GameInfo.devices, "activeDevices");
        SaveArray(tosterThoughts, "tosterThoughts");
        SaveArray(dialogues, "dialogues");
 //  PlayerPrefs.SetString("dialogue", JsonUtility.ToJson(dialogue, true));
   //print(PlayerPrefs.GetString("dialogue"));
        PlayerPrefs.SetString("tasks", JsonUtility.ToJson(taskManager.taskList, true));
     
        SaveObjectInfo(inventory.dictionary[1], "roboarm1");
        SaveObjectInfo(inventory.dictionary[2], "roboarm2");
        SaveObjectInfo(inventory.objectToSaveOnDelivery, "deliveryBotInventory");
      //  SaveObjectInfo(inventory.dictionary, "roboArmInventory");
        SaveObjectInfo(GameInfo.currentDevice, "currDevice");
      
        PlayerPrefs.Save();
    }

    private void SaveTasks() => PlayerPrefs.SetInt("CurrentDay", daysController.currentDay);
    
    private void SaveDevicesTransforms()
    {
        Transform T;
        
        T = deliveryBot.transform;
        PlayerPrefs.SetString("DeliveryPosition", $"{T.position.x} {T.position.y} {T.position.z}");
        PlayerPrefs.SetString("DeliveryRotation", $"{T.eulerAngles.x} {T.eulerAngles.y} {T.eulerAngles.z}");

        T = roboArmScript.gameObject.transform.root.transform;
        PlayerPrefs.SetString("RoboArmPosition", $"{T.position.x} {T.position.y} {T.position.z}");
        PlayerPrefs.SetFloat("RoboArmProgress", roboArmScript.progress);
        PlayerPrefs.SetString("RoboArmSpline", EditorJsonUtility.ToJson(roboArmScript.spline));

        T = beatle.transform;
        PlayerPrefs.SetString("BeatlePosition", $"{T.position.x} {T.position.y} {T.position.z}");
        PlayerPrefs.SetString("BeatleRotation", $"{T.eulerAngles.x} {T.eulerAngles.y} {T.eulerAngles.z}");
    }

    private void SaveObjectInfo<M>(M obj, string key)
    {
        PlayerPrefs.SetString(key, EditorJsonUtility.ToJson(obj));
    }
    //todo возможно не нужна вообще, а просто переделать под Json-ToJson 
    private void SaveArray<M>(M[] obj, string key)
    {
        for (int i = 0; i < obj.Length; i++)
        {
            PlayerPrefs.SetString(key+i, EditorJsonUtility.ToJson(obj[i]));
        }
    }

    public void Load()
    {

        LoadTasks();
        LoadDevicesTransfrom();
        // if (!String.IsNullOrEmpty(PlayerPrefs.GetString("roboArmInventory")))
        // {
        //     inventory.dictionary = (Dictionary<int, GameObject>) LoadObjectInfo(inventory.dictionary, "roboArmInventory");
        // }

     //   inventory.dictionary = JsonConvert.DeserializeObject<Dictionary<int, GameObject>>(PlayerPrefs.GetString("dictionary"));
     if (!String.IsNullOrEmpty(PlayerPrefs.GetString("roboarm1")))
     {
         inventory.dictionary[1] = new GameObject();
         inventory.dictionary[1] = (GameObject) LoadObjectInfo(inventory.dictionary[1], "roboarm1");
     }
     if (!String.IsNullOrEmpty(PlayerPrefs.GetString("roboarm2")))
     {
         inventory.dictionary[2] = new GameObject();
         inventory.dictionary[2] = (GameObject) LoadObjectInfo(inventory.dictionary[2], "roboarm2");
     }
     
        LoadArray(GameInfo.devices, "activeDevices");
        LoadArray(tosterThoughts, "tosterThoughts"); 
      //  EditorJsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("dialogue"), dialogue);
      
      DialogueTrigger d;
      
      for (int i = 0; i < dialogues.Length; i++)
          
      { print(JsonConvert.DeserializeObject<DialogueTrigger>(PlayerPrefs.GetString("dialogues"+i)).IsInteractable);
          print(PlayerPrefs.GetString("dialogues"+i));
           d = JsonConvert.DeserializeObject<DialogueTrigger>(PlayerPrefs.GetString("dialogues"+i));
          print(d.IsInterac table);
          dialogues[i].IsInteractable = d.IsInteractable;
      }
      
    // DialogueTrigger d = JsonConvert.DeserializeObject<DialogueTrigger>(PlayerPrefs.GetString("dialogue"));
    // dialogue.IsInteractable= d.IsInteractable;
      //  LoadArray(dialogues, "dialogues");

        if (PlayerPrefs.HasKey("tasks"))
        {
            taskManager.taskList =  JsonUtility.FromJson<List<TaskSO>>(PlayerPrefs.GetString("tasks"));
        }
        if (!String.IsNullOrEmpty(PlayerPrefs.GetString("deliveryBotInventory")))
        {
            inventory.objectToSaveOnDelivery = new GameObject();
            inventory.objectToSaveOnDelivery = (GameObject) LoadObjectInfo(inventory.objectToSaveOnDelivery, "deliveryBotInventory");
        }
        
       if (!String.IsNullOrEmpty(PlayerPrefs.GetString("currDevice")))
       {
           EventAggregator.DeviceSwitched.Publish( GameInfo.currentDevice = (GameObject) LoadObjectInfo(GameInfo.currentDevice, "currDevice"));
       }


    }

    private void LoadTasks()
    {
        EventAggregator.newDayStarted.Publish(daysController.currentDay = PlayerPrefs.HasKey("CurrentDay") ? PlayerPrefs.GetInt("CurrentDay") : 1);
        daysController.ObjectStateChange();
    }

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
        catch
        {
            Debug.LogError("No data about devices to load");
        }
    }

    private object LoadObjectInfo <H>(H obj, string key)
    { 
        EditorJsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), obj);
        print(PlayerPrefs.GetString(key));
        
        return  GameObject.Find((obj as GameObject).name);
    }
    
    private void LoadArray <H>(H[] obj, string key)
    {

        for (int i = 0; i < obj.Length; i++)
        {
            EditorJsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key+i), obj[i]);
        }
       
    }
}