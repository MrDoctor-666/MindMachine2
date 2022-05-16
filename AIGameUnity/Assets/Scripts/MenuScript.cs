using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuScript: MonoBehaviour
{
    [SerializeField] private DaysController dc;

    public void NewDay()
    {
        dc.NextDay();
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void QuitGame()
    {
        Application.Quit();
        Application.Quit();
    }
}