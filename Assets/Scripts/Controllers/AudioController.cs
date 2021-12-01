using System.Collections;
using System.Collections.Generic;
using AudioHelm;
using MidiJack;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

public class AudioController : MonoBehaviour
{
    private HelmController helmController;

    private void Awake()
    {
        helmController = GetComponent<HelmController>();
        MIDIController.NoteOn += PlaySound;
        MIDIController.NoteOff += NoteOff;
    }

    private void PlaySound(int note, float velocity)
    {
        helmController.NoteOn (note, velocity);
    }

    private void NoteOff(int note)
    {
        helmController.NoteOff (note);
    }
}
