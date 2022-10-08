using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(DeviceInfo))]
[RequireComponent(typeof(PlayerInput))]
public class CockroachRotation : MonoBehaviour
{
    [Header("Camera Rotation Settings")]
    public static float mouseSensitivityX = 0.5f;
    public static float mouseSensitivityY = 0.5f;
    [SerializeField] private float xClamp = 85f;

    private PlayerInput playerInput;
    private InputAction mouseXAction, mouseYAction;
    private DeviceInfo cockroachInfo;
    private Vector2 mouseInput;
    private float xRotation = 0f;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        cockroachInfo = gameObject.GetComponent<DeviceInfo>();

        mouseXAction = playerInput.currentActionMap.FindAction("MouseX");
        mouseYAction = playerInput.currentActionMap.FindAction("MouseY");
        mouseXAction.performed += OnMouseX;
        mouseYAction.performed += OnMouseY;
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

    public void Rotate()
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
}
