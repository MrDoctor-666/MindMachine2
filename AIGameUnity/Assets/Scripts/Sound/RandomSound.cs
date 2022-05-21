using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSound : MonoBehaviour
{
    [SerializeField] private List<AudioClip> sounds;
    private AudioSource audioSource;
    private SoundSource soundSource;

    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        soundSource = gameObject.GetComponent<SoundSource>();
    }

    private void Update()
    {
        OnRandomSound();
    }

    private void OnRandomSound()
    {
        if (soundSource.isFree)
        {
            if (sounds.Count != 0)
            {
                int range = Random.Range(0, sounds.Count);
                audioSource.clip = sounds[range];
                audioSource.Play();
                soundSource.PlayCounterTime();
            }
        }
    }
}
