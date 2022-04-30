public interface IInteractable
{
    float InteractionRadius { get; set; }
    bool MultipleUse { get; }
    bool IsInteractable { get; }
    bool IsPortable { get; }
    bool CanBePutOn { get; }
    bool CameraApproach { get; }
    string TooltipMessage { get; }

    void OnInteract();
}