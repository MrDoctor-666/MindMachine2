using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuScript: MonoBehaviour
{
    [SerializeField] private SaveManager sm;

    public void ReloadDay()
    {
        if (sm)
            sm.Load();
    }

    public void QuitToMenu()
    {
        EventAggregator.Reset();
        GameInfo.Reset();
        SceneManager.LoadScene("MainMenuScene");
    }
    
    public void QuitGame() => Application.Quit();
}