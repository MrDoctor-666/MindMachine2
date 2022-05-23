using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] GameObject main;
    [SerializeField] GameObject newGame;
    [SerializeField] GameObject settings;
    //[SerializeField] GameObject continueGame;


    public void StartScene(string name)
    {
        EventAggregator.Reset();
        GameInfo.Reset();
        SceneTransition.SwitchToScene(name);
    }

    public void StartNewGame(string name)
    {
        GameInfo.isUsingMarker = true;
        PlayerPrefs.DeleteAll();
        File.Delete (Application.dataPath +"/"+ "Save.xml");

        SceneTransition.SwitchToScene(name);
    }

    public void StartNewGameNoMarker(string name)
    {
        GameInfo.isUsingMarker = false;
        PlayerPrefs.DeleteAll();
        File.Delete(Application.dataPath + "/" + "Save.xml");

        SceneTransition.SwitchToScene(name);
    }

    public void NewGameDifficulty()
    {
        main.SetActive(false);
        newGame.SetActive(true);
    }

    public void OpenSettings()
    {
        settings.SetActive(true);
    }

    public void CloseSettings()
    {
        settings.SetActive(false);
    }

    public void Back()
    {
        main.SetActive(true);
        newGame.SetActive(false);
    }

    public void CloseApp() => Application.Quit(0);
}