using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DeliveryMove : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float gravity = 20f;
    [SerializeField] public int drag;
    [SerializeField] public bool isWithPlane;

    private DeviceInfo deviceInfoScript;
    private CharacterController chController;
    private DeviceInteraction deviceInteraction;
    private OnClickDevice onClickDevice;

    private InputAction moveInputAction;
    private InputAction takeInputAction;
    private InputAction deliveryInputAction;
    private InputAction planeInputAction;

    private Vector2 moveCommand;

    BoxCollider collider;
    private bool isMoving = false;
    private Vector3 move = Vector3.zero;
    private float initialGravity;
    private Vector3 fPoint, bPoint;

     [Header("Slope Handling")]
    [SerializeField] float backToNormalSpeed = 0.5f;
    float time = 0.0f;
    Quaternion endRotation, initialRotation;
    IEnumerator curCoroutine = null;


    private void Awake()
    {
        initialGravity = gravity;

        deviceInfoScript = gameObject.GetComponent<DeviceInfo>();
        onClickDevice = gameObject.GetComponent<OnClickDevice>();
        chController = GetComponent<CharacterController>();

        //playerInput.onActionTriggered += OnPlayerInputActionTriggered;
        moveInputAction = playerInput.currentActionMap.FindAction("Move");
        takeInputAction = playerInput.currentActionMap.FindAction("Take");
        deliveryInputAction = playerInput.currentActionMap.FindAction("Delivery");
        planeInputAction = playerInput.currentActionMap.FindAction("Plane");

        moveInputAction.performed += OnMove;
        moveInputAction.canceled += OnMove;

        takeInputAction.performed += OnTake;

        deliveryInputAction.performed += OnDelivery;
        deviceInteraction = GetComponent<DeviceInteraction>();

        planeInputAction.performed += OnPlane;
        planeInputAction.canceled += OnPlane;

        collider = GetComponent<BoxCollider>();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (deviceInfoScript.isActive)
        {
            moveCommand = context.action.ReadValue<Vector2>();
            if (!isMoving)
            {
                if (Mathf.Abs(moveCommand.x) > 0 || Mathf.Abs(moveCommand.y) > 0)
                {
                    EventAggregator.startMoving.Publish(gameObject);
                    isMoving = true;
                }
            }
            else
            {
                if (moveCommand == Vector2.zero)
                {
                    EventAggregator.endMoving.Publish(gameObject);
                    isMoving = false;
                }
            }
        }
        else
        {
            moveCommand = Vector2.zero;
        }
    }

    private void OnTake(InputAction.CallbackContext context)
    {
        // if (gameObject.GetComponent<DeviceInfo>().isActive)
        // {
        //     Debug.Log("IsActive");
        //     Ray ray = GetComponentInChildren<Camera>().ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        //     RaycastHit hit;
        //     Debug.DrawRay(ray.origin, ray.direction * 100f, Color.yellow);
        //     if (Physics.Raycast(ray, out hit))
        //     {
        //         if (hit.collider.gameObject.GetComponent<InteractableBase>())
        //         {
        //             hit.collider.gameObject.GetComponent<InteractableBase>().OnInteract();
        //         }
        //     }
        // }
        if (deviceInfoScript.isActive)
        {
            InteractableBase interact = InteractionData.interactionObj;
            if (interact != null)
                interact.OnInteract();
        }
    }

    private void OnDelivery(InputAction.CallbackContext context) { }

    private void OnPlane(InputAction.CallbackContext context)
    {   
        if (deviceInfoScript.isActive && isWithPlane)
        {
            if (planeInputAction.phase == InputActionPhase.Performed)
            {
                gravity = 5f;
            }

            if (planeInputAction.phase == InputActionPhase.Canceled)
            {
                gravity = initialGravity;
            } 
        }
    }

    private void FixedUpdate()
    {
        if (deviceInfoScript.isActive)
        {
            transform.rotation *= Quaternion.AngleAxis(Time.deltaTime * rotationSpeed * moveCommand.x, Vector3.up);
            //transform.position += transform.forward * movementSpeed * moveCommand.y * Time.deltaTime;
            //todo
           // Cursor.lockState = CursorLockMode.Locked;
            deviceInteraction.CheckForInteractionWithRayCastWithCursor(onClickDevice.InteractionRadius, 6, 9);

            if (chController.isGrounded)
            {
                //move wasd
                move = new Vector3(0, 0, moveCommand.y);
                move *= movementSpeed;
            }
            move.y -= gravity * Time.deltaTime;
            chController.Move(transform.rotation * move * Time.deltaTime);

            RotateToGround();
        }
        else if (!chController.isGrounded)
        {
            move = new Vector3(0, -gravity * Time.deltaTime, 0);
            chController.Move(transform.rotation * move * Time.deltaTime);
            //RotateToGround();
        }
    }

    private void RotateToGround()
    {
        float dist = 3f;
        Vector3 dir = -transform.up;
        RaycastHit hit, hit2;
        fPoint = transform.TransformPoint(collider.center + new Vector3(0, -collider.size.y / 2, collider.size.z / 2));
        bPoint = transform.TransformPoint(collider.center + new Vector3(0, -collider.size.y / 2, -collider.size.z / 2));

        Debug.DrawRay(fPoint, dir, Color.red);
        Debug.DrawRay(bPoint, dir, Color.red);
        int wallsLayer = 1 << 12, notRaycast = 1 << 9;
        int layerMask = ~(wallsLayer);

        if (Physics.Raycast(fPoint, dir, out hit, dist, layerMask) && Physics.Raycast(bPoint, dir, out hit2, dist, layerMask))
        {
            Vector3 upright = Vector3.Cross(transform.right, -(hit.point - hit2.point).normalized);
            transform.rotation = Quaternion.LookRotation(Vector3.Cross(transform.right, upright));
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Slope")
        {
            //gameObject.GetComponent<Rigidbody>().freezeRotation = false;
            if (curCoroutine != null) StopCoroutine(curCoroutine);
            curCoroutine = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Slope" && gameObject.GetComponent<Rigidbody>().freezeRotation == true)
        {
            //gameObject.GetComponent<Rigidbody>().freezeRotation = false;
            if (curCoroutine != null) StopCoroutine(curCoroutine);
            curCoroutine = null;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Slope")
        {
            //gameObject.GetComponent<Rigidbody>().freezeRotation = true;
            //transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
            if (curCoroutine != null) StopCoroutine(curCoroutine);
            curCoroutine = rotateSlowly();
            StartCoroutine(curCoroutine);
        }
    }

    IEnumerator rotateSlowly()
    {
        initialRotation = transform.rotation;
        endRotation = Quaternion.Euler(new Vector3(0, transform.localEulerAngles.y, 0));
        time = 0.0f;

        Debug.Log("Rotation: " + transform.eulerAngles.x + "   " + transform.eulerAngles.y);
        while (Mathf.Abs(transform.localEulerAngles.x) > 0.5 || Mathf.Abs(transform.localEulerAngles.z) > 0.5) {
            transform.rotation = Quaternion.Slerp(initialRotation, endRotation, time);
            time += Time.deltaTime / backToNormalSpeed;
            yield return new WaitForFixedUpdate();
        }

        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);

    }*/
}