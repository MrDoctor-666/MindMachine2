using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManagerScript : MonoBehaviour
{
    [SerializeField] private Canvas menu;

    private void Start()
    {
        menu.gameObject.SetActive(false);
        Cursor.visible = false;
    }
    
    public void CallMenu()
    {
        menu.gameObject.SetActive(!menu.gameObject.activeInHierarchy);

        Cursor.visible = menu.gameObject.activeInHierarchy;

        Time.timeScale = menu.gameObject.activeInHierarchy ? 0 : 1;
    }

    public void GoToMainMenuScene()
    {
        SceneManager.LoadScene("MainMenuScene");   
    }
}