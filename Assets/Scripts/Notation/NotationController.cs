using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotationController : MonoBehaviour
{
    public Transform playerPos;

    public List<Notation> visibleNotation = new List<Notation>();

    private static NotationController instance;

    private bool hasActiveNotation;

    public Notation activeNotation;

    private void Awake()
    {
        instance = this;
        MIDIController.NoteOn += CheckNote;
    }

    private void AddNotationToList(Notation notation)
    {
        visibleNotation.Add (notation);
    }

    public static void AddNotationToList_Static(Notation notation)
    {
        instance.AddNotationToList (notation);
    }

    private void RemoveNotationFromList(Notation notation)
    {
        visibleNotation.Remove (notation);
    }

    public static void RemoveNotationFromList_Static(Notation notation)
    {
        instance.RemoveNotationFromList (notation);
    }

    private void CheckNote(int note, float t)
    {
        List<Notation> duplicateNotations = new List<Notation>();

        note -= MIDIController.startingMIDINumber;

        if (hasActiveNotation)
        {
            if (activeNotation.notes[0] == note)
            {
                activeNotation.PlayNote();
            }
            else
            {
                //incorrect next note on active notation
            }
        }
        else
        {
            foreach (Notation notation in visibleNotation)
            {
                if (notation.notes[0] == note)
                {
                    duplicateNotations.Add (notation);
                }
                else
                {
                    //incorrect note accross all notes
                }
            }

            if (duplicateNotations.Count > 0)
            {
                var closetNotation =
                    GetClosetObject.GetClosetObj(duplicateNotations, playerPos);

                activeNotation = closetNotation;
                hasActiveNotation = true;
                activeNotation.HighlightNotation();
                activeNotation.PlayNote();
            }
        }

        if (hasActiveNotation && activeNotation.NotationFinished())
        {
            Destroy(activeNotation.gameObject);
            visibleNotation.Remove (activeNotation);
            hasActiveNotation = false;
            activeNotation = null;
        }
    }
}
