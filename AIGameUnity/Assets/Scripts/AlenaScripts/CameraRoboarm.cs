using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRoboarm : MonoBehaviour
{
    [SerializeField] private DeviceInfo roboarm;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float movementSpeed;
    [SerializeField] private int liftingHeight = 5;
    [SerializeField] private float maxInteractDistance = 100f;
    [SerializeField] private bool canUpDown;
    private bool under = true;
    private bool moveTowards = false;
   
    
    private Camera cam;
    private bool moveUpDownCommand = false;
    private bool action = false;
    private InputAction upDownInputAction;
    private InputAction approachAction;
    private InputAction defCameraInputAction;
    private Vector3 posForCam;
    private Vector3 defaultposForCam;
    private InteractableBase interact;
    private GameObject moveTo;
    [HideInInspector] public bool beReady = true;


    private void Awake()
    {
        cam = gameObject.GetComponent<Camera>();
        upDownInputAction = playerInput.currentActionMap.FindAction("UpDown");
        defCameraInputAction = playerInput.currentActionMap.FindAction("DefCamera");
        approachAction = playerInput.currentActionMap.FindAction("Approach");

        defCameraInputAction.performed += OnDefCamera;
        approachAction.performed += OnApproach;
        defaultposForCam = gameObject.transform.localPosition;
        if (canUpDown) upDownInputAction.performed += OnUpDown;

        defCameraInputAction.Disable();
    }

    void Update()
    {
        if (roboarm.isActive)
        {
            MovingUpDown();
            MovingToObject();
        }
        OnDefCamera();

    }

    private void OnUpDown(InputAction.CallbackContext context)
    {
        if (roboarm.isActive)
        {
            moveUpDownCommand = true;
        }
    }

    private void MovingUpDown()
    {
        if (moveUpDownCommand)
        {
            if (under)
            {
                if (transform.localPosition.y > liftingHeight)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - Time.deltaTime * movementSpeed, transform.localPosition.z);
                }
                else
                {
                    moveUpDownCommand = false;
                    under = false;
                }
            }
            else
            {
                if (transform.localPosition.y < defaultposForCam.y)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + Time.deltaTime * movementSpeed, transform.localPosition.z);
                }
                else
                {
                    moveUpDownCommand = false;
                    under = true;
                }
            }
        }
    }

    private void OnApproach(InputAction.CallbackContext context)
    {
        if (roboarm.isActive)
        {
            action = true;
            ApproachMoving();
        }
    }

    private void ApproachMoving()
    {
        if (action)
        {
            int layerMask1 = 1 << 8;
            int layerMask2 = 1 << 9;
            layerMask1 = layerMask1 | layerMask2;
            layerMask1 = ~layerMask1;
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);
            if (Physics.Raycast(ray, out hit, maxInteractDistance, layerMask1))
            {
                interact = hit.transform.GetComponent<InteractableBase>();
                Debug.Log(interact);
                if (interact == null) return;
                if (interact.CameraApproach == true)
                {
                    EventAggregator.endMoving.Publish(gameObject);
                    defCameraInputAction.Enable();
                    moveTo = hit.collider.gameObject;
                    Transform[] transformForCam = hit.transform.GetComponentsInChildren<Transform>();
                    if (transformForCam[1] != null)
                    { 
                        posForCam = transformForCam[1].position;
                        moveTowards = true;
                        moveTo.GetComponent<Collider>().enabled = false;
                    }
                }
            }
        }
    }

    private void MovingToObject()
    {
        if (moveTowards)
        {
            if (posForCam != defaultposForCam)
            {
                beReady = false;
                float step = movementSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, posForCam, step);

                if (Vector3.Distance(transform.position, posForCam) < 0.001f)
                {
                    action = false;
                    moveTowards = false;
                }
            }
        }
    }

    private void OnDefCamera(InputAction.CallbackContext context)
    {
        if (moveTowards == false && transform.localPosition != defaultposForCam)
        {
            transform.localPosition = defaultposForCam;
            moveTo.GetComponent<Collider>().enabled = true;
            beReady = true;
            defCameraInputAction.Disable();
        }
    }
    private void OnDefCamera()
    {
        if (!beReady && !roboarm.isActive)
        {
            transform.localPosition = defaultposForCam;
            moveTo.GetComponent<Collider>().enabled = true;
            beReady = true;
            defCameraInputAction.Disable();
        }
    }
}

