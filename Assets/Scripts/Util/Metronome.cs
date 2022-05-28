using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metronome : MonoBehaviour
{
    private double nextTime;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        nextTime = AudioSettings.dspTime + AudioController.secPerBeat;
    }

    void FixedUpdate()
    {
        if (AudioSettings.dspTime >= nextTime && audioSource.enabled)
        {
            audioSource.Play();
            nextTime += AudioController.secPerBeat;
        }
    }
}
