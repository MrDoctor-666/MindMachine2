using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{

    [SerializeField] PlayerInput playerInput;
    private InputAction escapeAction;

    private void Awake()
    {
        escapeAction = playerInput.currentActionMap.FindAction("Callmenu");
        escapeAction.performed += OnEscape;

    }

    public void OnEscape(InputAction.CallbackContext context)
    {
        EventAggregator.Reset();
        GameInfo.Reset();
        SceneManager.LoadScene("MainMenuScene");
    }
}
