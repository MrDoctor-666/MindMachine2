using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Text interactionText;
    [SerializeField] Text machinePowerText;
    [SerializeField] Text suspicionText;
    [SerializeField] Text uiText;

    [SerializeField] private GameObject daysCanvas;
    [SerializeField] private GameObject ErrorBuyPanel;
    [SerializeField] private GameObject ConfirmBuyPanel;
    [SerializeField] private GameObject TabMenu;
    [SerializeField] private GameObject EscMenu;
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
        EventAggregator.newDayStarted.Subscribe(OnDaysUI);

        EventAggregator.puzzleStarted.Subscribe(TurnOffAllUI);
        EventAggregator.puzzleEnded.Subscribe(TurnOnAllUI);

        //EventAggregator.endGame.Subscribe(End);

        mainGameCanvas = interactionText.transform.parent.GetComponent<Canvas>();
        demoEndGame = FindObjectInChildren(mainGameCanvas.gameObject, "DemoEndGame");
        
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
        if (currentOpen != null) EventAggregator.PanelClosed.Publish();
        else EventAggregator.PanelOpened.Publish(EscMenu);
        //open pause menu
    }

    public void OnInteractionEnter(GameObject device)
    {
        interactionText.text = InteractionData.interactionObj.TooltipMessage;
        interactionText.transform.position = Mouse.current.position.ReadValue();
    }

    public void OnInteractionExit(GameObject device)
    {
        interactionText.text = "";
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
        interactionText.text = "";
        InteractionData.Reset();
        GameInfo.currentDevice.GetComponentInChildren<Canvas>(true).gameObject.SetActive(false);
    }

    public void OnPanelOpened(GameObject panel)
    {
        currentOpen = panel;
        panel.SetActive(true);
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
    }

    void TurnOnAllUI(PuzzleEnd puzzEnd)
    {
        mainGameCanvas.gameObject.SetActive(true);
        puzzleCanvas.gameObject.SetActive(false);
        EventAggregator.PanelClosed.Publish();
        GameInfo.currentDevice.GetComponentInChildren<Canvas>(true).gameObject.SetActive(true);
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
