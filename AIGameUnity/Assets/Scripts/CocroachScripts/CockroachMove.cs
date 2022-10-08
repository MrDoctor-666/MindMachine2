using UnityEngine;
using UnityEngine.InputSystem;

public class CockroachMove : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float gravity = 20f;
    [SerializeField] private float jumpSpeed = 8f;

    private PlayerInput playerInput;
    private CharacterController chController;
    private DeviceInfo cockroachInfo;
    private DeviceInteraction deviceInteraction;
    private OnClickDevice onClickDevice;
    private InputAction moveActoin, takeInputAction, jumpAction;
    private CockroachRotation rotation;

    private Vector2 moveCommand;
    private Vector3 move = Vector3.zero;
    bool isMoving = false;
    bool jump = false;

    private void Awake()
    {
        cockroachInfo = gameObject.GetComponent<DeviceInfo>();
        deviceInteraction = gameObject.GetComponent<DeviceInteraction>();
        onClickDevice = gameObject.GetComponent<OnClickDevice>();
        chController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        rotation = GetComponent<CockroachRotation>();

        moveActoin = playerInput.currentActionMap.FindAction("Move");
        takeInputAction = playerInput.currentActionMap.FindAction("Take");
        jumpAction = playerInput.currentActionMap.FindAction("Jump");

        jumpAction.started += OnJump;
        moveActoin.performed += OnMove;
        moveActoin.canceled += OnMove;
        takeInputAction.performed += OnTake;

        //Vector3 sizeVec = GetComponent<Collider>().bounds.size;
    }

    private void OnTake(InputAction.CallbackContext context)
    {
        if (cockroachInfo.isActive)
        {
            InteractableBase interact = InteractionData.interactionObj;
            if (interact != null)
            {
                interact.OnInteract();
            }
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (cockroachInfo.isActive) moveCommand = context.action.ReadValue<Vector2>();
        else moveCommand = Vector2.zero;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (chController.isGrounded && cockroachInfo.isActive)
        {
            EventAggregator.endMoving.Publish(gameObject);
            EventAggregator.cockroachJump.Publish();
            isMoving = false;
            jump = true;
        }
    }

    void FixedUpdate()
    {
        CheckAnimation();
        if (cockroachInfo.isActive)
        {
            deviceInteraction.CheckForInteractionWithSphereCast(onClickDevice.InteractionRadius, 9);
            Cursor.lockState = CursorLockMode.Locked;

            if (chController.isGrounded)
            {
                //move wasd
                move = new Vector3(moveCommand.x, 0, moveCommand.y);
                move *= movementSpeed;
                //jump
                if (jump)
                {
                    move.y = jumpSpeed;
                    jump = false;
                }
            }
            move.y -= gravity * Time.deltaTime;
            chController.Move(transform.rotation * move * Time.deltaTime);
        }
        else if (!chController.isGrounded)
        {
            move = new Vector3(0, -gravity * Time.deltaTime, 0);
            chController.Move(transform.rotation * move * Time.deltaTime);
        }
        rotation.Rotate();
    }

    void CheckAnimation()
    {
        if (!chController.isGrounded) return;
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
}
