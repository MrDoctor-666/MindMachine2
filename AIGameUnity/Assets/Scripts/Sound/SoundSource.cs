using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSource : MonoBehaviour
{
    public bool isFree;
    private AudioSource audioSource;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    IEnumerator CountTime()
    {
        if (audioSource.clip != null)
        {
            yield return new WaitForSeconds(audioSource.clip.length);
            isFree = true;
        }
    }

    public void PlayCounterTime()
    {
        isFree = false;
        StartCoroutine(CountTime());
    }


}
