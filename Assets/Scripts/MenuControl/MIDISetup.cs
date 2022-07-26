using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class MIDISetup : MonoBehaviour
{
    [SerializeField]
    private Transform noteCircle;

    [SerializeField]
    private Transform keyboardHolder;

    [SerializeField]
    private int totalKeys;

    [SerializeField]
    private float startingXPos = -48.3f;

    [SerializeField]
    private float sharpYPosition;

    [SerializeField]
    private int minNote;

    [SerializeField]
    private float distX = 1.75f;

    private float maxNote;

    private int[] sharpNotes = { 1, 3, 6, 8, 10 };

    private List<Transform> noteCircles = new List<Transform>();

    private int startingMIDI;

    public float[] xPositions;

    void Awake()
    {
        MIDIController.NoteOn += SpawnNoteCircle;
        MIDIController.NoteOff += DestroyNoteCircle;

        startingMIDI = MIDIController.startingMIDINumber;
        maxNote = totalKeys - 1;

        xPositions = new float[totalKeys];

        float dist = 0;

        for (int i = 0; i < totalKeys; i++)
        {
            xPositions[i] = startingXPos + dist;
            dist += distX;
            if (i % 12 == 4 || i % 12 == 11) dist += distX + .3f;
        }
    }

    private void SpawnNoteCircle(int note, float vel)
    {
        int x = note - startingMIDI;

        if (minNote <= x && x <= maxNote)
        {
            float posY = -6;
            float posX = xPositions[note - startingMIDI];

            for (int i = 0; i < sharpNotes.Length; i++)
            if (note % 12 == sharpNotes[i]) posY = sharpYPosition;
            Vector3 newPos = new Vector3(posX, posY, 1);

            Transform newCircle = Instantiate(noteCircle, keyboardHolder);
            newCircle.localPosition = newPos;

            noteCircles.Add (newCircle);
        }
    }

    private void DestroyNoteCircle(int note)
    {
        int x = note - startingMIDI;
        if (minNote <= x && x <= maxNote)
        {
            float posY = -6;
            float posX = xPositions[note - startingMIDI];

            for (int i = 0; i < sharpNotes.Length; i++)
            if (note % 12 == sharpNotes[i]) posY = sharpYPosition;
            Vector3 circlePos = new Vector3(posX, posY, 1);

            for (int i = 0; i < noteCircles.Count; i++)
            {
                Vector3 newPosition = noteCircles[i].transform.localPosition;
                if (newPosition == circlePos)
                {
                    Destroy(noteCircles[i].gameObject);
                    noteCircles.RemoveAt (i);
                }
            }
        }
    }
}
