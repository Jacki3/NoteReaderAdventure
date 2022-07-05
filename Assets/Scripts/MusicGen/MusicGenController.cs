using System.Collections;
using System.Collections.Generic;
using AudioHelm;
using UnityEngine;

public class MusicGenController : MonoBehaviour
{
    public SequenceGenerator[] sequenceGenerators;

    public SampleSequencer[] drumMachines;

    void Start()
    {
        RegenerateMusic();
    }

    public void RegenerateMusic()
    {
        foreach (SequenceGenerator sequenceGenerator in sequenceGenerators)
        {
            sequenceGenerator.scale = CoreGameElements.i.easyNotes;
            sequenceGenerator.Generate();
        }

        int randomDrumIndex = Random.Range(0, drumMachines.Length);
        for (int i = 0; i < drumMachines.Length; i++)
        {
            drumMachines[i].enabled = false;
        }
        drumMachines[randomDrumIndex].enabled = true;
    }
}
