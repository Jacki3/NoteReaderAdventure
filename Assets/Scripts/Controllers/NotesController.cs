using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NotesController
{
    public static int[] pattern;

    private static List<int[]> possiblePatterns = new List<int[]>();

    public static int GetRandomNote(bool bass)
    {
        if (!bass)
        {
            int randIndex = Random.Range(0, CoreGameElements.i.notes.Length);
            int randNote = CoreGameElements.i.notes[randIndex];
            return randNote;
        }
        else
        {
            int randIndex =
                Random.Range(0, CoreGameElements.i.notesBass.Length);
            int randNote = CoreGameElements.i.notesBass[randIndex];
            return randNote;
        }
    }

    public static bool CanUsePattern(int totalNotesToSpawn, bool bass)
    {
        possiblePatterns.Clear();
        if (!bass)
        {
            for (int i = 0; i < CoreGameElements.i.patterns.Length; i++)
            {
                if (
                    CoreGameElements.i.patterns[i].pattern.Length ==
                    totalNotesToSpawn
                )
                {
                    possiblePatterns
                        .Add(CoreGameElements.i.patterns[i].pattern);
                }
            }
        }
        else
        {
            for (int i = 0; i < CoreGameElements.i.patternsBass.Length; i++)
            {
                if (
                    CoreGameElements.i.patternsBass[i].pattern.Length ==
                    totalNotesToSpawn
                )
                {
                    possiblePatterns
                        .Add(CoreGameElements.i.patternsBass[i].pattern);
                }
            }
        }
        if (possiblePatterns.Count > 0)
        {
            for (int i = 0; i < totalNotesToSpawn; i++)
            {
                int randIndex = Random.Range(0, possiblePatterns.Count);
                pattern = possiblePatterns[randIndex];
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}
