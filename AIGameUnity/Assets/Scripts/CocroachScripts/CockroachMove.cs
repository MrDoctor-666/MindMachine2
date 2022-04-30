using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CockroachMove : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed;
    [Header("Camera Rotation Settings")]
    [SerializeField] private float mouseSensitivityX = 1f;
    [SerializeField] private float mouseSensitivityY = 1f;
    [SerializeField] private float xClamp = 85f;

    Rigidbody rb;
    private DeviceInfo cockroachInfo;
    private DeviceInteraction deviceInteraction;
    private OnClickDevice onClickDevice;
    private Vector2 moveCommand;
    private Vector2 mouseInput;
    private InputAction mouseXAction, mouseYAction, moveActoin, takeInputAction;
    private float xRotation = 0f;
    bool isMoving = false;
    bool isBumping = false;
    CockroachJumpUpgrate jumpUpgrade;

    private void Awake()
    {
        cockroachInfo = gameObject.GetComponent<DeviceInfo>();
        deviceInteraction = gameObject.GetComponent<DeviceInteraction>();
        onClickDevice = gameObject.GetComponent<OnClickDevice>();
        rb = GetComponent<Rigidbody>();
        jumpUpgrade = GetComponentInChildren<CockroachJumpUpgrate>(true);
        //if (rb != null) rb.freezeRotation = true;

        mouseXAction = playerInput.currentActionMap.FindAction("MouseX");
        mouseYAction = playerInput.currentActionMap.FindAction("MouseY");
        moveActoin = playerInput.currentActionMap.FindAction("Move");
        takeInputAction = playerInput.currentActionMap.FindAction("Take");

        moveActoin.performed += OnMove;
        moveActoin.canceled += OnMove;
        mouseXAction.performed += OnMouseX;
        mouseYAction.performed += OnMouseY;
        takeInputAction.performed += OnTake;

        EventAggregator.cockroachJump.Subscribe(OnJump);
        //Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        Vector3 sizeVec = GetComponent<Collider>().bounds.size;
        gameObject.GetComponent<Rigidbody>().centerOfMass = new Vector3(0, -sizeVec.y, 0);
    }

    private void OnTake(InputAction.CallbackContext context)
    {
        if (cockroachInfo.isActive)
        {
            InteractableBase interact = InteractionData.interactionObj;
            if (interact != null)
            {
                interact.OnInteract();
                if (interact.MultipleUse == false) interact.ChangeIfInteractable();
            }
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (cockroachInfo.isActive) moveCommand = context.action.ReadValue<Vector2>();
        else moveCommand = Vector2.zero;
    }

    private void OnMouseX(InputAction.CallbackContext context)
    {
        if (cockroachInfo.isActive)
        {
            mouseInput.x = context.action.ReadValue<float>();
            mouseInput.x *= mouseSensitivityX;
        }
        else mouseInput.x = 0;
    }

    private void OnMouseY(InputAction.CallbackContext context)
    {
        if (cockroachInfo.isActive)
        {
            mouseInput.y = context.action.ReadValue<float>();
            mouseInput.y *= mouseSensitivityY;
            xRotation -= mouseInput.y;
            xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);
        }
        else mouseInput.y = 0;
    }

    void FixedUpdate()
    {
        CheckAnimation();
        if (cockroachInfo.isActive)
        {
            deviceInteraction.CheckForInteractionWithSphereCast(onClickDevice.InteractionRadius, 9);
            Cursor.lockState = CursorLockMode.Locked;

            //move wasd
            for (int i = 0; i < 1000; i++)
            {
                if (isBumping) { isBumping = false; break; }
                transform.position += transform.right * movementSpeed * moveCommand.x * Time.deltaTime / 1000;
                transform.position += transform.forward * movementSpeed * moveCommand.y * Time.deltaTime / 1000;
            }
        }
    }

    private void LateUpdate()
    {
        if (cockroachInfo.isActive)
        {
            //camera mouse x
            transform.Rotate(Vector3.up, mouseInput.x); //rotation
            //camera mouse y
            Camera mycam = GetComponentInChildren<Camera>();
            Vector3 targetRotation = mycam.transform.eulerAngles;
            targetRotation.x = xRotation + transform.eulerAngles.x; targetRotation.z = 0;
            mycam.transform.eulerAngles = targetRotation;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Ground" && collision.gameObject.tag != "Slope") isBumping = true;
        Vector3 dir = collision.contacts[0].point - transform.position;
        // We then get the opposite (-Vector3) and normalize it
        dir = -dir.normalized;
        Debug.Log(dir);
        dir.y = 0;

        // And finally we add force in the direction of dir and multiply it by force. 
        // This will push back the player
        rb.AddForce(dir * 3f);
        //rb.AddForce(-transform.forward * 100);
        //moveCommand = Vector3.zero;
    }

    void CheckAnimation()
    {
        if (!jumpUpgrade.isGrounded) return;
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
            if (moveCommand.x == 0 && moveCommand.y == 0)
            {
                EventAggregator.endMoving.Publish(gameObject);
                isMoving = false;
            }
        }
    }

    void OnJump()
    {
        isMoving = false;
        EventAggregator.endMoving.Publish(gameObject);
    }
}
