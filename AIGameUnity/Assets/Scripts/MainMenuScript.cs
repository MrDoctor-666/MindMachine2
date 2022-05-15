using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void StartScene(string name) => SceneTransition.SwitchToScene(name);

    public void StartNewGame(string name)
    {
        PlayerPrefs.DeleteAll();
        File.Delete (Application.dataPath +"/"+ "Save.xml");

        SceneTransition.SwitchToScene(name);
    }

    public void CloseApp() => Application.Quit(0);
}