using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Roboarm : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Camera cam;
    public static float mouseSensitivityX = 3f;
    public static float mouseSensitivityY = 0.2f;
    [SerializeField] private float xClamp = 85f;
    [HideInInspector] public GameObject objectToSave;
    [HideInInspector] public bool buttonChangeWay = false;

    [HideInInspector] public float maxInteractDistance; //lenght of roboarm
    public bool isArmEmpty = true;
    public bool onTheBorder = false;
    public int id;

    private DeviceInfo deviceInfo;
    private OnClickDevice onClickDevice;
    private Vector2 moveCommand;
    private Vector2 mouseInput;
    private InputAction moveInputAction;
    private InputAction mouseXAction, mouseYAction;
    private InputAction takeInputAction;
    private InputAction changeWayInputAction;
    private InputAction takeObjectOnSceneInputAction;
    private float xRotation = 0f;

    [Header ("Spline")]
    [SerializeField] public BezierSpline spline;
    [SerializeField] [Range(0, 1)] public float progress; //t on Bezier curve
    [Range(0, 1)] public float speed;
    public SplineWalkerMode mode;
    private bool goingForward = true;
    [HideInInspector] public float rail0To1;
    [HideInInspector] public float rail1To0;
    [HideInInspector] public BezierSpline[] splinesOfRails;
    private BezierSpline lastSpline;
    private DeviceInteraction deviceInteraction;

    private void Awake()
    {
        deviceInteraction = GetComponent<DeviceInteraction>();
        onClickDevice = GetComponent<OnClickDevice>();
        deviceInfo = gameObject.GetComponent<DeviceInfo>();
        GetComponentInChildren<Canvas>(true).gameObject.SetActive(true);

        moveInputAction = playerInput.currentActionMap.FindAction("Move");
        mouseXAction = playerInput.currentActionMap.FindAction("MouseX");
        mouseYAction = playerInput.currentActionMap.FindAction("MouseY");
        takeObjectOnSceneInputAction = playerInput.currentActionMap.FindAction("Action");
        changeWayInputAction = playerInput.currentActionMap.FindAction("ChangeWay");
        takeInputAction = playerInput.currentActionMap.FindAction("Take");

        moveInputAction.performed += OnMove;
        moveInputAction.canceled += OnMove;

        mouseXAction.performed += OnMouseX;
        mouseYAction.performed += OnMouseY;
        takeInputAction.performed += OnTake;
        takeObjectOnSceneInputAction.performed += OnTakeObjectOnScene;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;

        GetComponentInChildren<Text>().enabled = false;
        GetComponentInChildren<Image>().enabled = false;
        GetComponentInChildren<Canvas>().gameObject.SetActive(false);

        splinesOfRails = new BezierSpline[2];
        maxInteractDistance = onClickDevice.InteractionRadius;
    }

    private void Update()
    {
        if (deviceInfo.isActive)
        {   
            CurveDriving();
            CameraRotation();

            if (buttonChangeWay)
            {
                EventAggregator.sceneButtonEvent.Publish();
                ChangeWay();
                buttonChangeWay = false;
            }

            Cursor.lockState = CursorLockMode.Locked;
            deviceInteraction.CheckForInteractionWithRayCast(maxInteractDistance, 8, 9);
        }
    }

    private void CameraRotation()
    {
        Vector3 rotY = cam.transform.localEulerAngles;
        rotY.x = xRotation;
        cam.transform.localEulerAngles = rotY;
    }

    private void CurveDriving()
    {
        if (deviceInfo.isActive && cam.GetComponent<CameraRoboarm>().beReady)
        {
            float duration = 1 / speed;
            float cameraRotation = cam.transform.rotation.y;
            float a = -2 * cameraRotation + 1;

            if (moveCommand.y > 0)
            {
                progress += (a * Time.deltaTime * moveCommand.y / duration);
                if (progress > 1f) //the last point of curve
                {
                    if (mode == SplineWalkerMode.Once)
                    {
                        progress = 1f;
                    }
                    else if (mode == SplineWalkerMode.Loop)
                    {
                        progress -= 1f;
                    }
                }
            }
            else if (moveCommand.y < 0)
            {
                progress += (a * Time.deltaTime * moveCommand.y / duration);
                if (progress < 0f)
                {
                    progress = -progress;
                }
            }
        }

        Vector3 position = spline.GetPoint(progress);
        transform.localPosition = position;
        transform.LookAt(position + spline.GetDirection(progress));
    }

    public void LenghtUpgrade(float lenght)
    {
        onClickDevice.InteractionRadius = lenght;
        maxInteractDistance = onClickDevice.InteractionRadius;
    }

    public void ButtonUpgrade()
    {
        changeWayInputAction.performed += OnChangeWay;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (deviceInfo.isActive && cam.GetComponent<CameraRoboarm>().beReady)
        {
            moveCommand = context.action.ReadValue<Vector2>();
            if (moveCommand != Vector2.zero)
            {
                EventAggregator.startMoving.Publish(gameObject);
            }
            else
            {
                EventAggregator.endMoving.Publish(gameObject);
            }
        }
        else 
        {
            moveCommand = Vector2.zero;
        }
    }

    private void OnTakeObjectOnScene(InputAction.CallbackContext context)
    {
        TakeObjectWithCheck();
    }

    private void TakeObjectWithCheck()
    {
        if (deviceInfo.isActive)
        {
            EventAggregator.takeObjectEvent.Publish(gameObject);
            EventAggregator.inventoryImageEvent.Publish(gameObject);
        }
    }

    private void OnChangeWay(InputAction.CallbackContext context)
    {
        ChangeWay();
    }

    private void ChangeWay()
    {
        lastSpline = spline;
        if (onTheBorder)
        {
            if (lastSpline == splinesOfRails[0])
            {
                var localSpline1 = splinesOfRails[1].GetComponent<BezierSpline>();
                if (localSpline1 != null) spline = localSpline1;
                progress = rail0To1;
                EventAggregator.changeWayEvent.Publish();
            }
            else if (lastSpline == splinesOfRails[1])
            {
                var localSpline0 = splinesOfRails[0].GetComponent<BezierSpline>();
                if (localSpline0 != null) spline = localSpline0;
                progress = rail1To0;
                EventAggregator.changeWayEvent.Publish();
            }
        }
    }
    private void OnTake(InputAction.CallbackContext context)
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        InteractableBase interact = InteractionData.interactionObj;
         /*if (interact != null)
         {
             interact.OnInteract();
             if (interact.MultipleUse == false) interact.ChangeIfInteractable();
         }*/

        if (gameObject.GetComponent<DeviceInfo>().isActive)
        {
            Debug.Log("IsActive");
            Ray ray = GetComponentInChildren<Camera>().ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.yellow);
            if (Physics.Raycast(ray, out hit, maxInteractDistance, layerMask))
            {
                if (hit.collider.gameObject.GetComponent<InteractableBase>() &&
                    hit.collider.gameObject.GetComponent<InteractableBase>().IsInteractable &&
                    hit.collider.gameObject.GetComponent<InteractableBase>().CanInteract())
                {
                    hit.collider.gameObject.GetComponent<InteractableBase>().OnInteract();
                }
            }
        }
    }

    private void OnMouseX(InputAction.CallbackContext context)
    {
        if (deviceInfo.isActive)
        {
            mouseInput.x = context.action.ReadValue<float>();
            mouseInput.x *= mouseSensitivityX;

            cam.transform.Rotate(Vector3.up * mouseInput.x * Time.deltaTime * mouseSensitivityX, Space.World);
        }
    }
    
    private void OnMouseY(InputAction.CallbackContext context)
    {
        if (deviceInfo.isActive)
        {
            mouseInput.y = context.action.ReadValue<float>();
            mouseInput.y *= mouseSensitivityY;

            xRotation -= mouseInput.y;
            xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);
            cam.transform.localEulerAngles = new Vector3(xRotation, cam.transform.localEulerAngles.y, cam.transform.localEulerAngles.z);
        }
    } 
    
    /*private void OnDelivery(InputAction.CallbackContext context)
    {

        Debug.Log("Delivery action");
        Ray ray = GetComponentInChildren<Camera>().ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.yellow);
        if (Physics.Raycast(ray, out hit, maxInteractDistance))
        {
            InteractableBase interact = hit.transform.GetComponent<InteractableBase>();
            if (interact == null) return;
            if (isEmpty && (interact.IsPortable == true))
            {
                objectToSave = hit.collider.gameObject;
                objectToSave.SetActive(false);
                isEmpty = false;
                Debug.Log("IsTaken");
            }
            else if ((isEmpty==false) && (interact.CanBePutOn == true))
            {
                if (objectToSave == null)
                {
                    return;
                }
                objectToSave.transform.position = hit.point;
                objectToSave.SetActive(true);
                isEmpty = true;
                Debug.Log("Done");
            }
        }
    }*/
}
