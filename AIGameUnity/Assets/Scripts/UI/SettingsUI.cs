using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup masterVolume;
    private float constant = 25;

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
        CockroachMove.mouseSensitivityX = 0.05f+sense*0.6f;
        Roboarm.mouseSensitivityX = 0.2f+sense*2.1f;
    }
    public void ChangeSensitivityY(float sense)
    {
        CockroachMove.mouseSensitivityY = 0.02f+sense*0.6f;
        Roboarm.mouseSensitivityY = 0.01f+sense*0.235f;
    }
}
