using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] 
    List<UsableButton> buttons = new List<UsableButton>();
    Animator animator;
    
    
    bool isOpen = false, isInAnimation = false;
    float animTime;

    private void Start()
    {
        animator = GetComponent<Animator>();

        foreach (UsableButton button in buttons)
            button.AssignUsedObject(Use);

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
            Debug.Log(clip.name + clip.length);
        animTime = clips[0].length;

    }

    public void Use()
    {
        if (!isInAnimation) StartCoroutine(playAnimation());
    }

    IEnumerator playAnimation()
    {
        isInAnimation = true;
        //animation
        if (!isOpen)
            animator.Play("DoorOpen");
        else animator.Play("DoorClose");
        EventAggregator.sceneDoorsEvent.Publish();

        yield return new WaitForSeconds(animTime);
        isOpen = !isOpen;
        isInAnimation = false;
        Debug.Log("Door is open? " + isOpen);
    }
}
