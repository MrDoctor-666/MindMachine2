
using UnityEngine;

public class DeliveryBotAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;

    private void Awake()
    {
        EventAggregator.startMoving.Subscribe(OnStartMoving);
        EventAggregator.endMoving.Subscribe(OnEndMoving);
    }

    private void OnStartMoving(GameObject device)
    {
        if (device.GetComponent<DeliveryMove>() != null)
        {
            animator.enabled = true;
        }
    }

    private void OnEndMoving(GameObject device)
    {
        if (device.GetComponent<DeliveryMove>() != null)
        {
            animator.enabled = false;
        }
    }
}
