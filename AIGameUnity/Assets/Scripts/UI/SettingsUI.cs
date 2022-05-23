using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup masterVolume;
    [SerializeField] private CockroachMove cockroach;
    [SerializeField] private Roboarm roboarm;

    public void ToggleMusic(bool enabled)
    {
        if (enabled) masterVolume.audioMixer.SetFloat("MasterVolume", 0);
        else masterVolume.audioMixer.SetFloat("MasterVolume", -80);
    }

    public void ChangeVolume(float volume)
    {
        masterVolume.audioMixer.SetFloat("MasterVolume", Mathf.Lerp(-30,0,volume));
    }

    public void ChangeSensitivity(float volume)
    {

    }
}
