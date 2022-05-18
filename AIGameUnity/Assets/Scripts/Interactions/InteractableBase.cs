using UnityEngine;

public class InteractableBase : MonoBehaviour, IInteractable
{
    [Space, Header("Interactable Settings")]
    [SerializeField] private float interactionRadius = 1f;
    [SerializeField] private bool multipleUse = false;
    [SerializeField] private bool isInteractable = true;
    [SerializeField] private bool isPortable = false;
    [SerializeField] private bool canBePutOn = false;
    [SerializeField] private bool cameraApproach = false;
    [SerializeField] protected string tooltipMessage = "Interact";

    public bool MultipleUse => multipleUse;
    public bool IsInteractable
    {
        get => isInteractable;
        set => isInteractable = value;
    }
    public bool IsPortable => isPortable;
    public bool CanBePutOn => canBePutOn;
    public bool CameraApproach => cameraApproach;
    public string TooltipMessage => tooltipMessage;

    public float InteractionRadius
    { 
        get => interactionRadius;
        set => interactionRadius = value;
    }

    public virtual void OnInteract()
    {
        Debug.Log("Interacted: " + name);
        EventAggregator.IntercationAreaExited.Publish(gameObject);
        InteractionData.Reset();
    }

    public void ChangeIfInteractable()
    {
        isInteractable = !isInteractable;
    }

    public void SwitchTooltipMessage(string newMessage)
    {
        tooltipMessage = newMessage;
    }

    public virtual bool CanInteract()
    {
        return true;
    }
}
