﻿using System.Collections;
using System.Collections.Generic;
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
        switch (CoreGameElements.i.currentDifficulty)
        {
            case CoreGameElements.Difficuties.easy:
                scale = CoreGameElements.i.easyNotes;
                break;
            case CoreGameElements.Difficuties.medium:
                scale = CoreGameElements.i.mediumNotes;
                break;
            case CoreGameElements.Difficuties.hard:
                scale = CoreGameElements.i.hardNotes;
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
