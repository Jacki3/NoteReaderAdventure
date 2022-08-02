using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metronome : MonoBehaviour
{
    private double nextTime;

    private AudioSource audioSource;

    private float currentVol;

    private static Metronome i;

    private void Awake()
    {
        i = this;
        audioSource = GetComponent<AudioSource>();
        currentVol = audioSource.volume;
        Mute();
    }

    void Start()
    {
        nextTime = AudioSettings.dspTime + AudioController.secPerBeat;
    }

    void FixedUpdate()
    {
        if (AudioSettings.dspTime >= nextTime && audioSource.enabled)
        {
            if (GameStateController.state != GameStateController.States.Paused)
                audioSource.Play();
            nextTime += AudioController.secPerBeat;
        }
    }

    public static void UnMuteMetro()
    {
        i.UnMute();
    }

    public static void MuteMetro()
    {
        i.Mute();
    }

    private void Mute()
    {
        audioSource.volume = 0;
    }

    private void UnMute()
    {
        audioSource.volume = currentVol;
    }
}
