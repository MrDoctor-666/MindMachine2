using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CockroachJumpUpgrate : MonoBehaviour
{
    [SerializeField] float jumpForce = 5f;
    //Rigidbody rb;
    PlayerInput playerInput;
    InputAction jumpAction;
    public bool isGrounded;
    private void OnEnable()
    {
        //rb = transform.parent.GetComponent<Rigidbody>();
        playerInput = transform.parent.GetComponent<PlayerInput>();
        jumpAction = playerInput.currentActionMap.FindAction("Jump");

        jumpAction.started += OnJump;

        EventAggregator.DialogueEnded.Subscribe(OnEndDialogue);
    }

    private void OnJump(InputAction.CallbackContext context)
    {

        if (isGrounded && transform.parent.GetComponent<DeviceInfo>().isActive)
        {
            //EventAggregator.endMoving.Publish(transform.parent.gameObject);
            EventAggregator.cockroachJump.Publish();
            //rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnEndDialogue()
    {
        isGrounded = false;
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.2f);
        isGrounded = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.tag == "Ground") 
        if (other.gameObject != transform.parent.gameObject)    
            isGrounded = true;

    }
}
