using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] TextMeshProUGUI interactionText;
    [SerializeField] Text machinePowerText;
    [SerializeField] Text suspicionText;
    [SerializeField] Text uiText;

    [SerializeField] private GameObject daysCanvas;
    [SerializeField] private GameObject ErrorBuyPanel;
    [SerializeField] private GameObject ConfirmBuyPanel;
    [SerializeField] private GameObject TabMenu;
    [SerializeField] private GameObject EscMenu;
    [SerializeField] private GameObject SettingsMenu;
    [SerializeField] private GameObject dialoguePanel;

    GameObject demoEndGame;

    Canvas mainGameCanvas;
    Canvas puzzleCanvas;
    private GameObject currentOpen = null;
    private InputAction escapeAction;
    private InputAction tabAction;

    private void Awake()
    {
        EventAggregator.IntercationAreaEntered.Subscribe(OnInteractionEnter);
        EventAggregator.IntercationAreaExited.Subscribe(OnInteractionExit);
        EventAggregator.DeviceBuyError.Subscribe(OnBuyError);
        EventAggregator.DeviceBuyTried.Subscribe(OnBuyTry);
        EventAggregator.PanelOpened.Subscribe(OnPanelOpened);
        EventAggregator.PanelClosed.Subscribe(OnPanelClosed);
        EventAggregator.DialogueStarted.Subscribe(OnDialogueStarted);
        EventAggregator.newDayStarted.Subscribe(OnDaysUI);

        EventAggregator.puzzleStarted.Subscribe(TurnOffAllUI);
        EventAggregator.puzzleEnded.Subscribe(TurnOnAllUI);

        //EventAggregator.endGame.Subscribe(End);

        mainGameCanvas = interactionText.transform.parent.parent.parent.GetComponent<Canvas>();
        demoEndGame = FindObjectInChildren(mainGameCanvas.gameObject, "DemoEndGame");
        dialoguePanel = FindObjectInChildren(mainGameCanvas.gameObject, "DialoguePanel");
        
        puzzleCanvas = gameObject.GetComponentInChildren<Canvas>(true);
        ErrorBuyPanel.SetActive(false);
        ConfirmBuyPanel.SetActive(false);

        escapeAction = playerInput.currentActionMap.FindAction("Escape");
        escapeAction.performed += OnEscape;

        tabAction = playerInput.currentActionMap.FindAction("Tab");
        tabAction.performed += OnTab;
    }
    private void Start()
    {
        OnDaysUI(1);
    }

    private void Update()
    {
        PowerUI();
    }

    private void OnDaysUI(int day)
    {
        daysCanvas.SetActive(true);
        uiText.text = "День " + day;
    }
    private void PowerUI()
    {
        machinePowerText.text = GameInfo.computingPower.ToString();
        suspicionText.text = GameInfo.suspicion.ToString();
    }

    public void OnEscape(InputAction.CallbackContext context)
    {
        if (currentOpen != null /*&& currentOpen != dialoguePanel*/)
        {
            EventAggregator.PanelClosed.Publish();
            SettingsMenu.SetActive(false);
        }
        else if (currentOpen == null) EventAggregator.PanelOpened.Publish(EscMenu);
        //open pause menu
    }

    public void OnBack() //override for back button in UI
    {
        SettingsMenu.SetActive(false);
        EscMenu.SetActive(true);
    }

    public void OnSettings()
    {
        SettingsMenu.SetActive(true);
        EscMenu.SetActive(false);
    }

    public void OnInteractionEnter(GameObject device)
    {
        interactionText.text = InteractionData.interactionObj.TooltipMessage;
        interactionText.transform.parent.gameObject.SetActive(true);
        interactionText.transform.parent.transform.position = Mouse.current.position.ReadValue();
        //interactionText.transform.position = Mouse.current.position.ReadValue();
    }

    public void OnInteractionExit(GameObject device)
    {
        interactionText.text = "";
        interactionText.transform.parent.gameObject.SetActive(false);
    }

    public void OnBuyError(GameObject device)
    {
        ErrorBuyPanel.SetActive(true);
        currentOpen = ErrorBuyPanel;
        GameInfo.currentDevice.GetComponentInChildren<Canvas>(true).gameObject.SetActive(false);
    }

    public void OnBuyTry(GameObject device)
    {
        ConfirmBuyPanel.SetActive(true);
        currentOpen = ConfirmBuyPanel;
        EventAggregator.IntercationAreaExited.Publish(device);
        InteractionData.Reset();
        GameInfo.currentDevice.GetComponentInChildren<Canvas>(true).gameObject.SetActive(false);
    }

    public void OnDialogueStarted(GameObject d)
    {
        OnPanelOpened(dialoguePanel);
        GameInfo.currentDevice.GetComponentInChildren<Canvas>(true).gameObject.SetActive(false);
    }

    public void OnPanelOpened(GameObject panel)
    {
        currentOpen = panel;
        panel.SetActive(true);
        EventAggregator.IntercationAreaExited.Publish(GameInfo.currentDevice);
        GameInfo.currentDevice.GetComponentInChildren<Canvas>(true).gameObject.SetActive(false);
    }

    public void OnPanelClosed()
    {
        if (currentOpen != null) currentOpen.SetActive(false);
        currentOpen = null;
        GameInfo.currentDevice.GetComponentInChildren<Canvas>(true).gameObject.SetActive(true);
    }

    public void OnTab(InputAction.CallbackContext context)
    {
        if (currentOpen == null) EventAggregator.PanelOpened.Publish(TabMenu);
        else if (currentOpen == TabMenu) EventAggregator.PanelClosed.Publish();
        //OnPanelOpened(TabMenu);
    }

    void TurnOffAllUI(GameObject device)
    {
        mainGameCanvas.gameObject.SetActive(false);
        puzzleCanvas.gameObject.SetActive(true);
        GameInfo.currentDevice.GetComponentInChildren<Canvas>(true).gameObject.SetActive(false);
        escapeAction.Disable();
        tabAction.Disable();
    }

    void TurnOnAllUI(PuzzleEnd puzzEnd)
    {
        mainGameCanvas.gameObject.SetActive(true);
        puzzleCanvas.gameObject.SetActive(false);
        EventAggregator.PanelClosed.Publish();
        GameInfo.currentDevice.GetComponentInChildren<Canvas>(true).gameObject.SetActive(true);
        escapeAction.Enable();
        tabAction.Enable();
    }

    void End()
    {
        EventAggregator.PanelOpened.Publish(demoEndGame);
    }



    GameObject FindObjectInChildren(GameObject parent, string name)
    {
        Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }
}
