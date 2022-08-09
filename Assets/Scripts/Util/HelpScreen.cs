using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpScreen : MonoBehaviour
{
    [SerializeField]
    private Transform noteMarker;

    [SerializeField]
    private Transform markerParent;

    [SerializeField]
    private int totalKeys;

    [SerializeField]
    private float startingXPos;

    [SerializeField]
    private float sharpYPosition;

    [SerializeField]
    private int minNote;

    [SerializeField]
    private float distX;

    [SerializeField]
    private ControlSchema[] controlSchemas;

    [Serializable]
    private class ControlSchema
    {
        public ControlSchemas schema;

        public Image schemaObject;
    }

    [SerializeField]
    private Color highlightColour;

    private enum ControlSchemas
    {
        Up,
        Left,
        Right,
        Down,
        Escape,
        Interact,
        ReadMode
    }

    private float maxNote;

    private int[] sharpNotes = { 1, 3, 6, 8, 10 };

    private List<Transform> noteMarkers = new List<Transform>();

    private int startingMIDI;

    private float[] xPositions;

    private void Awake()
    {
        MIDIController.NoteOn += SpawnNoteMarker;
        MIDIController.NoteOff += DestroyNoteMarker;

        startingMIDI = MIDIController.startingMIDINumber;
        maxNote = totalKeys - 1;

        xPositions = new float[totalKeys];

        float dist = 0;

        for (int i = 0; i < totalKeys; i++)
        {
            xPositions[i] = startingXPos + dist;
            dist += distX;
            if (i % 12 == 4 || i % 12 == 11) dist += distX * .75f;
        }
    }

    private void Update()
    {
        if (RigidPlayerController.inputActions.Player.Up.IsPressed())
            HighLightSchema(ControlSchemas.Up, true);
        else
            HighLightSchema(ControlSchemas.Up, false);
        if (RigidPlayerController.inputActions.Player.Down.IsPressed())
            HighLightSchema(ControlSchemas.Down, true);
        else
            HighLightSchema(ControlSchemas.Down, false);
        if (RigidPlayerController.inputActions.Player.Left.IsPressed())
            HighLightSchema(ControlSchemas.Left, true);
        else
            HighLightSchema(ControlSchemas.Left, false);
        if (RigidPlayerController.inputActions.Player.Right.IsPressed())
            HighLightSchema(ControlSchemas.Right, true);
        else
            HighLightSchema(ControlSchemas.Right, false);
        if (RigidPlayerController.inputActions.Player.ReadModeSwitch.IsPressed()
        )
            HighLightSchema(ControlSchemas.ReadMode, true);
        else
            HighLightSchema(ControlSchemas.ReadMode, false);
        if (RigidPlayerController.inputActions.Player.Escape.IsPressed())
            HighLightSchema(ControlSchemas.Escape, true);
        else
            HighLightSchema(ControlSchemas.Escape, false);
        if (RigidPlayerController.inputActions.UI.Submit.IsPressed())
            HighLightSchema(ControlSchemas.Interact, true);
        else
            HighLightSchema(ControlSchemas.Interact, false);
    }

    private void HighLightSchema(ControlSchemas controlSchema, bool highLight)
    {
        foreach (ControlSchema _controlSchema in controlSchemas)
        {
            if (controlSchema == _controlSchema.schema)
            {
                Color newColour = highLight ? highlightColour : Color.white;
                _controlSchema.schemaObject.color = newColour;
            }
        }
    }

    private void SpawnNoteMarker(int note, float vel)
    {
        int x = note - startingMIDI;

        if (minNote <= x && x <= maxNote)
        {
            float posY = noteMarker.transform.localPosition.y;
            float posX = xPositions[note - startingMIDI];

            for (int i = 0; i < sharpNotes.Length; i++)
            if (note % 12 == sharpNotes[i]) posY = sharpYPosition;
            Vector3 newPos = new Vector3(posX, posY, 1);

            Transform newMarker = Instantiate(noteMarker, markerParent);
            newMarker.localPosition = newPos;

            noteMarkers.Add (newMarker);
        }
    }

    private void DestroyNoteMarker(int note)
    {
        int x = note - startingMIDI;
        if (minNote <= x && x <= maxNote)
        {
            float posY = noteMarker.transform.localPosition.y;
            float posX = xPositions[note - startingMIDI];

            for (int i = 0; i < sharpNotes.Length; i++)
            if (note % 12 == sharpNotes[i]) posY = sharpYPosition;
            Vector3 circlePos = new Vector3(posX, posY, 1);

            for (int i = 0; i < noteMarkers.Count; i++)
            {
                Vector3 newPosition = noteMarkers[i].transform.localPosition;
                if (newPosition == circlePos)
                {
                    Destroy(noteMarkers[i].gameObject);
                    noteMarkers.RemoveAt (i);
                }
            }
        }
    }
}
