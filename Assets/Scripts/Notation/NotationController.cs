﻿using System.Collections;
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
        hasActiveNotation = false;
        activeNotation = null;
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
                ScoreController.AddStreak_Static();
            }
            else if (note != 25)
            {
                //should be reading circle button NOT 25
                ScoreController.ResetStreak_Static();
                activeNotation.IncorrecNote();
                //keep counting incorrect notes and deactivate on certain amount?
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
            }

            if (duplicateNotations.Count > 0)
            {
                var closetNotation =
                    GetClosetObject.GetClosetObj(duplicateNotations, playerPos);

                var parent = closetNotation.transform.root;

                var child = parent.GetChild(0).GetComponent<Notation>();

                if (child.notes[0] == note)
                {
                    ScoreController.AddStreak_Static();
                    activeNotation = child;
                    hasActiveNotation = true;
                    activeNotation.HighlightNotation();
                    activeNotation.PlayNote();
                }
            }
            else if (note != 25 && PlayerController.readingMode)
            {
                //incorrect note accross all notes
                //should be reading circle button NOT 25
                //repeating what the notation script does?
                ScoreController.ResetStreak_Static();

                foreach (Notation notation in visibleNotation)
                {
                    notation.IncorrecNote();
                }
            }
        }

        if (hasActiveNotation && activeNotation.NotationFinished())
        {
            visibleNotation.Remove (activeNotation);
            hasActiveNotation = false;
            activeNotation = null;
        }
    }
}