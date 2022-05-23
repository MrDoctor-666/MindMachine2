using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup masterVolume;
    [SerializeField] private float constant = 25;

    public void ToggleMusic(bool enabled)
    {
        if (enabled) masterVolume.audioMixer.SetFloat("MasterVolume", 0);
        else masterVolume.audioMixer.SetFloat("MasterVolume", -80);
    }

    public void ChangeVolume(float volume)
    {
        masterVolume.audioMixer.SetFloat("MasterVolume", Mathf.Lerp(-30,0,volume));
    }

    public void ChangeSensitivityX(float sense)
    {
        CockroachMove.mouseSensitivityX = sense;
        Roboarm.mouseSensitivityX = constant*sense;
    }
    public void ChangeSensitivityY(float sense)
    {
        CockroachMove.mouseSensitivityY = sense;
        Roboarm.mouseSensitivityY = sense;
    }
}
