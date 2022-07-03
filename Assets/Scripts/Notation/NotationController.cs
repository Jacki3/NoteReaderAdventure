using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotationController : MonoBehaviour
{
    public Transform playerPos;

    public int resetAmount;

    public int incorrectNotesToDamage;

    public List<Notation> visibleNotation = new List<Notation>();

    private static NotationController instance;

    private bool hasActiveNotation;

    private Notation activeNotation;

    private int incorrectNotes;

    private void Awake()
    {
        instance = this;
        MIDIController.NoteOn += CheckNote;
    }

    private void AddNotationToList(Notation notation)
    {
        if (!visibleNotation.Contains(notation)) visibleNotation.Add(notation);
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
        if (Application.loadedLevel == 0)
        {
            List<Notation> duplicateNotations = new List<Notation>();

            note -= MIDIController.startingMIDINumber;

            if (hasActiveNotation)
            {
                if (activeNotation.notes[0] == note)
                {
                    incorrectNotes = 0;
                    activeNotation.PlayNote();
                    ScoreController.AddStreak_Static();
                }
                else if (note != 25)
                {
                    //should be reading circle button NOT 25
                    ScoreController.ResetStreak_Static(false);
                    activeNotation.IncorrecNote();
                    incorrectNotes++;
                    if (incorrectNotes >= resetAmount)
                    {
                        activeNotation.UnhighlightNotation();
                        hasActiveNotation = false;
                        activeNotation = null;
                    }
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
                        GetClosetObject
                            .GetClosetObj(duplicateNotations, playerPos);

                    ScoreController.AddStreak_Static();
                    activeNotation = closetNotation;
                    hasActiveNotation = true;
                    activeNotation.HighlightNotation();
                    activeNotation.PlayNote();

                    incorrectNotes = 0;
                }
                else if (note != 25 && PlayerController.readingMode)
                {
                    //incorrect note accross all notes
                    //should be reading circle button NOT 25
                    //repeating what the notation script does?
                    if (visibleNotation.Count > 0)
                        ScoreController.ResetStreak_Static(false);

                    foreach (Notation notation in visibleNotation)
                    {
                        //why is this not just incorrectNote? Why a whole nother animation?
                        incorrectNotes++;
                        if (incorrectNotes > incorrectNotesToDamage)
                        {
                            HealthController.RemoveHealth(1, true);
                            incorrectNotes = 0;
                        }
                        notation.AllIncorrect();
                    }
                }
            }

            if (hasActiveNotation && activeNotation.NotationFinished())
            {
                incorrectNotes = 0;
                visibleNotation.Remove (activeNotation);
                hasActiveNotation = false;
                activeNotation = null;
            }

            if (AudioController.canPlay)
            {
                ScoreController
                    .AddRhythmScore_Static(CoreGameElements.i.scoreForRhythm);
                ScoreController.AddStreak_Static();
            }
            else
                ScoreController.ResetStreak_Static(true);
        }
    }

    private void DestroyPlayedNote(int note)
    {
        if (hasActiveNotation)
        {
            activeNotation.DestroyPlayedNote (note);
        }
        else
        {
            foreach (Notation notation in visibleNotation)
            {
                notation.DestroyPlayedNote (note);
            }
        }
    }
}
