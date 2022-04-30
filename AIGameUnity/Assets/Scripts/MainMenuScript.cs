using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void StartScene(string name) => SceneTransition.SwitchToScene(name);

    public void StartNewGame(string name)
    {
        PlayerPrefs.DeleteAll();
            
        SceneTransition.SwitchToScene(name);
    }

    public void CloseApp() => Application.Quit(0);
}