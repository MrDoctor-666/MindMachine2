using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DeviceInteraction : MonoBehaviour
{
   // [Header("Interact Ray Settings")]
    //[SerializeField] float maxInteractDistance = 100f;
    [SerializeField] float raySphereRadius = 0.5f;
    [SerializeField] string textForWarningDelivery = "��� ������������ �����������. �������������� ������ �����������.";

    public void CheckForInteractionWithSphereCast(float interactDist, int layerNum)
    {
        int layerMask = 1 << layerNum;
        layerMask = ~layerMask;
        Camera cam = GetComponentInChildren<Camera>();
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.yellow);

        if (Physics.SphereCast(ray, raySphereRadius,out hit, interactDist,layerMask))
        {
            InteractableBase interact = hit.transform.GetComponent<InteractableBase>();
            //null or outside of any interaction range
            if (interact == null || hit.distance > interact.InteractionRadius)
            {
                //if before we were in the interaction range
                if (InteractionData.interactionObj != null)
                {
                    EventAggregator.IntercationAreaExited.Publish(gameObject);
                    InteractionData.Reset();
                }
                return;
            }
            //if it's the same object as last time
            if (interact == InteractionData.interactionObj) return;
            //if we're in range of interactable object and it's the first time
            if (interact.IsInteractable && interact.CanInteract() && !interact.CanBePutOn)
            {
                //show ui
                InteractionData.interactionObj = interact;
                EventAggregator.IntercationAreaEntered.Publish(gameObject);
                return;
            }
            EventAggregator.IntercationAreaExited.Publish(gameObject);
            InteractionData.Reset();
        }
        else if (InteractionData.interactionObj != null)
        {
            InteractionData.Reset();
            EventAggregator.IntercationAreaExited.Publish(gameObject);
        }
    }
    
    public void CheckForInteractionWithRayCast(float interactDist, int layerNum1, int layerNum2)
    {
        int layerMask1 = 1 << layerNum1;
        int layerMask2 = 1 << layerNum2;
        layerMask1 = layerMask1 | layerMask2;
        layerMask1 = ~layerMask1;
        Camera cam = GetComponentInChildren<Camera>();
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.yellow);

        if (Physics.Raycast(ray, out hit, interactDist, layerMask1))
        {
            InteractableBase interact = hit.transform.GetComponent<InteractableBase>();
            //null or outside of any interaction range
            if (interact == null || hit.distance > interact.InteractionRadius)
            {
                //if before we were in the interaction range
                if (InteractionData.interactionObj != null)
                {
                    EventAggregator.IntercationAreaExited.Publish(gameObject);
                    InteractionData.Reset();
                }
                return;
            }
            //if it's the same object as last time
            if (interact == InteractionData.interactionObj) return;
            //if we're in range of interactable object and it's the first time
            if (interact.IsInteractable && interact.CanInteract())
            {
                //show ui
                InteractionData.interactionObj = interact;
                EventAggregator.IntercationAreaEntered.Publish(gameObject);
                return;
            }
            EventAggregator.IntercationAreaExited.Publish(gameObject);
            InteractionData.Reset();
        }
        else if (InteractionData.interactionObj != null)
        {
            InteractionData.Reset();
            EventAggregator.IntercationAreaExited.Publish(gameObject);
        }
    }
    public void CheckForInteractionWithRayCastWithCursor(float interactDist, int layerNum1, int layerNum2)
    {
        int layerMask1 = 1 << layerNum1;
        int layerMask2 = 1 << layerNum2;
        layerMask1 = layerMask1 | layerMask2;
        layerMask1 = ~layerMask1;
        Camera cam = GetComponentInChildren<Camera>();
       // Ray ray = new Ray(cam.transform.position, cam.transform.forward);
       Ray ray = cam.ScreenPointToRay (Mouse.current.position.ReadValue());
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.yellow);
        
        if (Physics.Raycast(ray, out hit, interactDist, layerMask1))
        {
            if (hit.transform.CompareTag("DeliveryBot"))
            {
                return;
            }
            InteractableBase interact = hit.transform.GetComponent<InteractableBase>();
            //null or outside of any interaction range
            if (interact == null || (hit.distance - 3f) > interact.InteractionRadius)
            {
                //if before we were in the interaction range
                if (InteractionData.interactionObj != null)
                {
                    EventAggregator.IntercationAreaExited.Publish(gameObject);
                    InteractionData.Reset();
                }
                return;
            }
            //if it's the same object as last time
            if (interact == InteractionData.interactionObj) return;
            //if we're in range of interactable object and it's the first time
            if (interact.IsInteractable)
            {
                if (interact.CanBePutOn)
                {
                    EventAggregator.TempPanelOpened.Publish(textForWarningDelivery);
                    return;
                }
                //show ui
                InteractionData.interactionObj = interact;
                EventAggregator.IntercationAreaEntered.Publish(gameObject);
                return;
            }
            EventAggregator.IntercationAreaExited.Publish(gameObject);
            InteractionData.Reset();
        }
        else if (InteractionData.interactionObj != null)
        {
            InteractionData.Reset();
            EventAggregator.IntercationAreaExited.Publish(gameObject);
        }
    }
}
