using System.Collections;
using System.Collections.Generic;
using AudioHelm;
using MidiJack;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

public class AudioController : MonoBehaviour
{
    public Canvas mainCanvas;

    public RhythmBar[] rhythmBars;

    public HelmController helmControllerMovement;

    private HelmController helmController;

    private AudioHelmClock helmClock;

    public static float secPerBeat;

    public static bool canPlay;

    private float time;

    private void Awake()
    {
        helmController = GetComponent<HelmController>();
        helmClock = GetComponent<AudioHelmClock>();

        MIDIController.NoteOn += PlaySound;
        MIDIController.NoteOff += NoteOff;
    }

    private void Start()
    {
        StartCoroutine(SpawnIndicator());
    }

    void Update()
    {
        secPerBeat = 60 / helmClock.bpm;

        time = Mathf.Lerp(time, 0, secPerBeat * Time.deltaTime);

        if (time < .5f)
            canPlay = true;
        else
            canPlay = false;
    }

    private IEnumerator SpawnIndicator()
    {
        while (true)
        {
            foreach (RhythmBar bar in rhythmBars)
            {
                var _bar = Instantiate(bar, mainCanvas.transform);
            }
            yield return new WaitForSeconds(secPerBeat);
            time = 1;
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
