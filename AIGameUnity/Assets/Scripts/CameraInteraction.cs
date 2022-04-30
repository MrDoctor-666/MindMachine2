using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraInteraction : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    private InputAction takeInputAction;
    private DeviceInfo deviceInfo;
    private Camera cam;

    private void Awake()
    {
        Debug.Log(Mouse.current.position.ReadValue());
        deviceInfo = gameObject.GetComponent<DeviceInfo>();
        cam = GetComponentInChildren<Camera>();
        takeInputAction = playerInput.currentActionMap.FindAction("Take");
        takeInputAction.performed += OnTake;
    }

    private void Update()
    {
        if (deviceInfo.isActive)
        {
            CheckForInteraction();
        }
    }

    private void OnTake(InputAction.CallbackContext context)
    {
        if (deviceInfo.isActive)
        {
            int layerMask = 1 << 9;
            layerMask = ~layerMask;
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.yellow);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                if (hit.collider.gameObject.GetComponent<InteractableBase>())
                {
                    hit.collider.gameObject.GetComponent<InteractableBase>().OnInteract();
                }
            }
        }
    }

    public void CheckForInteraction()
    {
        int layerMask = 1 << 9;
        layerMask = ~layerMask;
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.yellow);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            InteractableBase interact = hit.transform.GetComponent<InteractableBase>();
            if (InteractionData.interactionObj == interact) return;
            if (InteractionData.interactionObj != null || (interact == gameObject.GetComponent<InteractableBase>() && gameObject.GetComponent<InteractableBase>() != null))
            {
                InteractionData.Reset();
                EventAggregator.IntercationAreaExited.Publish(gameObject);
                return;
            }
            InteractionData.interactionObj = interact;
            EventAggregator.IntercationAreaEntered.Publish(gameObject);
        }
        else if (InteractionData.interactionObj != null)
        {
            InteractionData.Reset();
            EventAggregator.IntercationAreaExited.Publish(gameObject);
        }
    }
}
