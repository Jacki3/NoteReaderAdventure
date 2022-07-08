﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AudioHelm;
using UnityEngine;

public class MusicGenController : MonoBehaviour
{
    public SequenceGenerator[] sequenceGenerators;

    public SampleSequencer[] drumMachines;

    private static MusicGenController i;

    private int randomDrumIndex;

    private void Awake()
    {
        i = this;
    }

    public static void RegenMusic()
    {
        i.RegenerateMusic();
    }

    private void RegenerateMusic()
    {
        int[] scale = null;
        switch (CoreGameElements.i.currentDifficultyNotes)
        {
            case CoreGameElements.DifficutiesNotes.one:
                scale = CoreGameElements.i.allNotes.oneNotes.ToArray();
                break;
            case CoreGameElements.DifficutiesNotes.two:
                scale = CoreGameElements.i.allNotes.twoNotes.ToArray();
                break;
            case CoreGameElements.DifficutiesNotes.three:
                scale = CoreGameElements.i.allNotes.threeNotes.ToArray();
                break;
            case CoreGameElements.DifficutiesNotes.four:
                scale = CoreGameElements.i.allNotes.fourNotes.ToArray();
                break;
            case CoreGameElements.DifficutiesNotes.five:
                scale = CoreGameElements.i.allNotes.fiveNotes.ToArray();
                break;
            case CoreGameElements.DifficutiesNotes.six:
                scale = CoreGameElements.i.allNotes.sixNotes.ToArray();
                break;
            case CoreGameElements.DifficutiesNotes.seven:
                scale = CoreGameElements.i.allNotes.sevenNotes.ToArray();
                break;
            case CoreGameElements.DifficutiesNotes.eight:
                scale = CoreGameElements.i.allNotes.eightNotes.ToArray();
                break;
            case CoreGameElements.DifficutiesNotes.nine:
                scale = CoreGameElements.i.allNotes.nineNotes.ToArray();
                break;
            case CoreGameElements.DifficutiesNotes.ten:
                scale = CoreGameElements.i.allNotes.tenNotes.ToArray();
                break;
            case CoreGameElements.DifficutiesNotes.eleven:
                scale = CoreGameElements.i.allNotes.elevenNotes.ToArray();
                break;
            case CoreGameElements.DifficutiesNotes.twelve:
                scale = CoreGameElements.i.allNotes.twelveNotes.ToArray();
                break;
            case CoreGameElements.DifficutiesNotes.thirteen:
                scale = CoreGameElements.i.allNotes.thirteenNotes.ToArray();
                break;
            case CoreGameElements.DifficutiesNotes.fourteen:
                scale = CoreGameElements.i.allNotes.fourteenNotes.ToArray();
                break;
            case CoreGameElements.DifficutiesNotes.fifteen:
                scale = CoreGameElements.i.allNotes.fifteenNotes.ToArray();
                break;
            case CoreGameElements.DifficutiesNotes.sixteen:
                scale = CoreGameElements.i.allNotes.sixteenNotes.ToArray();
                break;
            case CoreGameElements.DifficutiesNotes.seventeen:
                scale = CoreGameElements.i.allNotes.seventeenNotes.ToArray();
                break;
        }
        for (int i = 0; i < scale.Length; i++) scale[i] %= 12;
        foreach (SequenceGenerator sequenceGenerator in sequenceGenerators)
        {
            if (scale.Length <= 0) scale = sequenceGenerator.scale;
            sequenceGenerator.scale = scale;
            sequenceGenerator.Generate();
        }

        randomDrumIndex = Random.Range(0, drumMachines.Length);
        for (int i = 0; i < drumMachines.Length; i++)
        {
            drumMachines[i].enabled = false;
        }
        // drumMachines[randomDrumIndex].enabled = true;
    }

    public static void DisableMusic()
    {
        i.DisableAllMusic();
    }

    private void DisableAllMusic()
    {
        foreach (SequenceGenerator sequenceGenerator in sequenceGenerators)
        {
            sequenceGenerator.scale = null;
            sequenceGenerator.sequencer.Clear();
        }
        for (int i = 0; i < drumMachines.Length; i++)
        {
            drumMachines[i].enabled = false;
        }
    }

    public static void MuteDrums()
    {
        i.DisableDrums();
    }

    private void DisableDrums()
    {
        for (int i = 0; i < drumMachines.Length; i++)
        {
            drumMachines[i].enabled = false;
        }
    }

    public static void UnMuteDrums()
    {
        i.EnableDrums();
    }

    private void EnableDrums()
    {
        drumMachines[randomDrumIndex].enabled = true;
    }
}
