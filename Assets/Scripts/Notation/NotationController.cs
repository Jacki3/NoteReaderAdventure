﻿using System.Collections;
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
            if (
                GameStateController.state == GameStateController.States.Play ||
                GameStateController.state == GameStateController.States.Tutorial
            )
            {
                List<Notation> duplicateNotations = new List<Notation>();

                if (hasActiveNotation)
                {
                    if (activeNotation.notes[0] == note)
                    {
                        incorrectNotes = 0;
                        activeNotation.PlayNote();
                        ScoreController.AddStreak_Static();
                        CoreGameElements.i.currentNoteStreak++;
                    }
                    else if (note != 61)
                    {
                        //should be reading circle button
                        CoreGameElements.i.currentNoteStreak = 0;

                        //update UI
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

                        CoreGameElements.i.currentNoteStreak++;
                        ScoreController.AddStreak_Static();
                        activeNotation = closetNotation;
                        hasActiveNotation = true;
                        activeNotation.HighlightNotation();
                        activeNotation.PlayNote();

                        incorrectNotes = 0;
                    }
                    else if (note != 61 && RigidPlayerController.readingMode)
                    {
                        //incorrect note accross all notes
                        //should be reading circle button NOT 25
                        //repeating what the notation script does?
                        if (visibleNotation.Count > 0)
                        {
                            //update UI
                            CoreGameElements.i.currentNoteStreak = 0;
                            ScoreController.ResetStreak_Static(false);
                        }
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
            }

            int savedNoteStreak = CoreGameElements.i.gameSave.noteStreak;
            int currentNoteStreak = CoreGameElements.i.currentNoteStreak;

            UIController
                .UpdateTextUI(UIController.UITextComponents.currentStreak,
                currentNoteStreak.ToString());

            if (currentNoteStreak > savedNoteStreak)
            {
                CoreGameElements.i.gameSave.noteStreak = currentNoteStreak;
                UIController
                    .UpdateTextUI(UIController.UITextComponents.noteStreak,
                    currentNoteStreak.ToString());
            }

            if (AudioController.canPlay && CoreGameElements.i.usingRhythm)
            {
                ScoreController
                    .AddRhythmScore_Static(CoreGameElements.i.scoreForRhythm);
                ScoreController.AddStreak_Static();

                CoreGameElements.i.currentRhythmStreak++;
                UIController
                    .UpdateTextUI(UIController
                        .UITextComponents
                        .currentRhythmStreak,
                    CoreGameElements.i.currentRhythmStreak.ToString());

                int savedRhythmStreak =
                    CoreGameElements.i.gameSave.rhythmStreak;
                int currentRhythmStreak =
                    CoreGameElements.i.currentRhythmStreak;
                if (currentRhythmStreak > savedRhythmStreak)
                {
                    CoreGameElements.i.gameSave.rhythmStreak =
                        currentRhythmStreak;
                    UIController
                        .UpdateTextUI(UIController
                            .UITextComponents
                            .rhythmStreak,
                        currentRhythmStreak.ToString());
                }
            }
            else if (CoreGameElements.i.usingRhythm)
            {
                ScoreController.ResetStreak_Static(true);
                CoreGameElements.i.currentRhythmStreak = 0;
                UIController
                    .UpdateTextUI(UIController
                        .UITextComponents
                        .currentRhythmStreak,
                    CoreGameElements.i.currentRhythmStreak.ToString());
            }
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
