using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    public Characters character;
    public Animator animator;
    public bool isInAnimation = false;
    void Start()
    {
        //animator = GetComponent<Animator>();
        //start idle animation
    }

    public void PlayAnimation(string name)
    {
        Debug.Log(gameObject.name + name);
        animator.SetTrigger(name);
        Debug.Log(isInAnimation);
        //StartCoroutine(OnCompleteAnimation());
    } 

    IEnumerator OnCompleteAnimation()
    {
        int count = (int)animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        Debug.Log("In transition: " + animator.IsInTransition(0));
        Debug.Log("Animation start" + character + animator.GetCurrentAnimatorStateInfo(0).IsName("CombatIdle")); 
        isInAnimation = true;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        /*while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime % count < 1f)
        {
            Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            yield return null; 
        }*/

        Debug.Log("In transition: " + animator.IsInTransition(0));
        isInAnimation = false;
        Debug.Log("Animation end" + character);
    }
}
