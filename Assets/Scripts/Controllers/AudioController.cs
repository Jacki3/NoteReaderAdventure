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

    void Start()
    {
        InputSystem.onDeviceChange += (device, change) =>
        {
            if (change != InputDeviceChange.Added) return;

            var midiDevice = device as Minis.MidiDevice;
            if (midiDevice == null) return;

            midiDevice.onWillNoteOn += (note, velocity) =>
            {
                PlaySound(note.noteNumber, velocity);
            };

            midiDevice.onWillNoteOff += (note) =>
            {
                NoteOff(note.noteNumber);
            };
        };
    }

    private void Awake()
    {
        helmController = GetComponent<HelmController>();
    }

    private void PlaySound(int note, float velocity)
    {
        print (note);
        helmController.NoteOn (note, velocity);
    }

    private void NoteOff(int note)
    {
        helmController.NoteOff (note);
    }
}
