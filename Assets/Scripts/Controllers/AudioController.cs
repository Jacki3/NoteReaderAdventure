using System.Collections;
using System.Collections.Generic;
using AudioHelm;
using MidiJack;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

public class AudioController : MonoBehaviour
{
    public Transform mainCanvas;

    public RhythmBar[] rhythmBars;

    public HelmController helmControllerMovement;

    private HelmController helmController;

    private AudioHelmClock helmClock;

    public static float secPerBeat;

    public static bool canPlay;

    private float time;

    private double nextTime;

    private bool dontspawn = false;

    private void Awake()
    {
        helmController = GetComponent<HelmController>();
        helmClock = GetComponent<AudioHelmClock>();

        MIDIController.NoteOn += PlaySound;
        MIDIController.NoteOff += NoteOff;
    }

    private void Start()
    {
        nextTime = AudioSettings.dspTime + secPerBeat;
    }

    void FixedUpdate()
    {
        if (AudioSettings.dspTime >= nextTime)
        {
            SpawnMetronomeBar();
            nextTime += secPerBeat;
            time = 1;
        }
    }

    void Update()
    {
        secPerBeat = 60 / helmClock.bpm;

        time = Mathf.Lerp(time, 0, secPerBeat * Time.fixedDeltaTime);

        if (time < .5f)
            canPlay = true;
        else
            canPlay = false;
    }

    private void SpawnMetronomeBar()
    {
        print("spawning");
        foreach (RhythmBar bar in rhythmBars)
        {
            var _bar = Instantiate(bar, mainCanvas);
        }
    }

    private void PlaySound(int note, float velocity)
    {
        //hard code - should be equal to the note used for reading circle in input manager
        if (PlayerController.readingMode && note != 61)
            helmController.NoteOn(note, velocity);
        else
            helmControllerMovement.NoteOn(note, velocity);
    }

    private void NoteOff(int note)
    {
        helmController.NoteOff (note);
        helmControllerMovement.NoteOff (note);
    }
}
