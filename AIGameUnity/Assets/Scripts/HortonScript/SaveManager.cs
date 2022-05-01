using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
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
        // SaveArray(dialogues, "dialogues");
        SaveObjectInfo(inventory.dictionary[1], "roboarm1");
        SaveObjectInfo(inventory.dictionary[2], "roboarm2");
        SaveObjectInfo(inventory.objectToSaveOnDelivery, "deliveryBotInventory");
        SaveObjectInfo(GameInfo.currentDevice, "currDevice");
        SaveDialogues(); // check this

        PlayerPrefs.Save();
    }

    private void SaveTasks()
    {
        PlayerPrefs.SetInt("CurrentDay", daysController.currentDay);
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
        PlayerPrefs.SetString("RoboArmSpline", EditorJsonUtility.ToJson(roboArmScript.spline));

        T = beatle.transform;
        PlayerPrefs.SetString("BeatlePosition", $"{T.position.x} {T.position.y} {T.position.z}");
        PlayerPrefs.SetString("BeatleRotation", $"{T.eulerAngles.x} {T.eulerAngles.y} {T.eulerAngles.z}");
    }

    private void SaveObjectInfo<M>(M obj, string key) => PlayerPrefs.SetString(key, EditorJsonUtility.ToJson(obj));

    //todo возможно не нужна вообще, а просто переделать под Json-ToJson 
    private void SaveArray<M>(M[] obj, string key)
    {
        for (int i = 0; i < obj.Length; i++)
            PlayerPrefs.SetString(key+i, EditorJsonUtility.ToJson(obj[i]));
    }

    private void SaveDialogues()
    {
        for (int i = 0; i < dialogues.Length; ++i)
            PlayerPrefs.SetInt($"dialogue{i}", dialogues[i].IsInteractable ? 1 : 0);
    }

    public void Load()
    {
        LoadTasks();
        LoadDevicesTransfrom();
        LoadRoboArmInventory();
        LoadDeliveryBotInventory();
        LoadCurrentDevice();
        LoadDialoguesState(); // check this
        LoadArray(GameInfo.devices, "activeDevices");
        LoadArray(tosterThoughts, "tosterThoughts");
    }

    private void LoadTasks()
    {
        EventAggregator.newDayStarted.Publish(daysController.currentDay = PlayerPrefs.HasKey("CurrentDay") ? PlayerPrefs.GetInt("CurrentDay") : 1);
        daysController.ObjectStateChange();

        if (PlayerPrefs.HasKey("tasks"))
            taskManager.taskList = JsonUtility.FromJson<List<TaskSO>>(PlayerPrefs.GetString("tasks"));
    }

    private void LoadCurrentDevice()
    {
        if (PlayerPrefs.HasKey("currDevice") && !string.IsNullOrEmpty(PlayerPrefs.GetString("currDevice")))
            EventAggregator.DeviceSwitched.Publish(GameInfo.currentDevice = LoadObjectInfo(GameInfo.currentDevice, "currDevice"));
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
        catch { Debug.LogError("SaveManager.LoadDevicesTransfrom(): no data about devices to load"); }
    }

    private void LoadRoboArmInventory()
    {
        if (!(PlayerPrefs.HasKey("roboarm1") && PlayerPrefs.HasKey("roboarm2")))
            return;

        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("roboarm1")))
        {
            inventory.dictionary[1] = new GameObject();
            inventory.dictionary[1] = LoadObjectInfo(inventory.dictionary[1], "roboarm1");
        }
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("roboarm2")))
        {
            inventory.dictionary[2] = new GameObject();
            inventory.dictionary[2] = LoadObjectInfo(inventory.dictionary[2], "roboarm2");
        }
    }

    private void LoadDeliveryBotInventory()
    {
        if (PlayerPrefs.HasKey("deliveryBotInventory") && !string.IsNullOrEmpty(PlayerPrefs.GetString("deliveryBotInventory")))
        {
            inventory.objectToSaveOnDelivery = new GameObject();
            inventory.objectToSaveOnDelivery = LoadObjectInfo(inventory.objectToSaveOnDelivery, "deliveryBotInventory");
        }
    }

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

    private GameObject LoadObjectInfo <H>(H obj, string key)
    { 
        EditorJsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), obj);

        return obj as GameObject;
    }
    
    private void LoadArray <H>(H[] obj, string key)
    {
        for (int i = 0; i < obj.Length; i++) 
            EditorJsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key+i), obj[i]);
    }
}