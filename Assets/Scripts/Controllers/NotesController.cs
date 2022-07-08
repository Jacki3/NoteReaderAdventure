using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NotesController
{
    public static int[] pattern;

    private static List<int[]> possiblePatterns = new List<int[]>();

    public static int GetRandomNote(bool bass)
    {
        switch (CoreGameElements.i.currentDifficultyNotes)
        {
            case CoreGameElements.DifficutiesNotes.one:
                {
                    int randIndex =
                        Random
                            .Range(0,
                            CoreGameElements.i.allNotes.oneNotes.Length);
                    int randNote =
                        CoreGameElements.i.allNotes.oneNotes[randIndex];
                    return randNote;
                }
            case CoreGameElements.DifficutiesNotes.two:
                {
                    int randIndex =
                        Random
                            .Range(0,
                            CoreGameElements.i.allNotes.twoNotes.Length);
                    int randNote =
                        CoreGameElements.i.allNotes.twoNotes[randIndex];
                    return randNote;
                }
            case CoreGameElements.DifficutiesNotes.three:
                {
                    int randIndex =
                        Random
                            .Range(0,
                            CoreGameElements.i.allNotes.threeNotes.Length);
                    int randNote =
                        CoreGameElements.i.allNotes.threeNotes[randIndex];
                    return randNote;
                }
            case CoreGameElements.DifficutiesNotes.four:
                {
                    int randIndex =
                        Random
                            .Range(0,
                            CoreGameElements.i.allNotes.fourNotes.Length);
                    int randNote =
                        CoreGameElements.i.allNotes.fourNotes[randIndex];
                    return randNote;
                }
            case CoreGameElements.DifficutiesNotes.five:
                {
                    int randIndex =
                        Random
                            .Range(0,
                            CoreGameElements.i.allNotes.fiveNotes.Length);
                    int randNote =
                        CoreGameElements.i.allNotes.fiveNotes[randIndex];
                    return randNote;
                }
            case CoreGameElements.DifficutiesNotes.six:
                {
                    int randIndex =
                        Random
                            .Range(0,
                            CoreGameElements.i.allNotes.sixNotes.Length);
                    int randNote =
                        CoreGameElements.i.allNotes.sixNotes[randIndex];
                    return randNote;
                }
            case CoreGameElements.DifficutiesNotes.seven:
                {
                    int randIndex =
                        Random
                            .Range(0,
                            CoreGameElements.i.allNotes.sevenNotes.Length);
                    int randNote =
                        CoreGameElements.i.allNotes.sevenNotes[randIndex];
                    return randNote;
                }
            case CoreGameElements.DifficutiesNotes.eight:
                {
                    int randIndex =
                        Random
                            .Range(0,
                            CoreGameElements.i.allNotes.eightNotes.Length);
                    int randNote =
                        CoreGameElements.i.allNotes.eightNotes[randIndex];
                    return randNote;
                }
            case CoreGameElements.DifficutiesNotes.nine:
                {
                    int randIndex =
                        Random
                            .Range(0,
                            CoreGameElements.i.allNotes.nineNotes.Length);
                    int randNote =
                        CoreGameElements.i.allNotes.nineNotes[randIndex];
                    return randNote;
                }
            case CoreGameElements.DifficutiesNotes.ten:
                {
                    int randIndex =
                        Random
                            .Range(0,
                            CoreGameElements.i.allNotes.tenNotes.Length);
                    int randNote =
                        CoreGameElements.i.allNotes.tenNotes[randIndex];
                    return randNote;
                }
            case CoreGameElements.DifficutiesNotes.eleven:
                {
                    int randIndex =
                        Random
                            .Range(0,
                            CoreGameElements.i.allNotes.elevenNotes.Length);
                    int randNote =
                        CoreGameElements.i.allNotes.elevenNotes[randIndex];
                    return randNote;
                }
            case CoreGameElements.DifficutiesNotes.twelve:
                {
                    int randIndex =
                        Random
                            .Range(0,
                            CoreGameElements.i.allNotes.twelveNotes.Length);
                    int randNote =
                        CoreGameElements.i.allNotes.twelveNotes[randIndex];
                    return randNote;
                }
            case CoreGameElements.DifficutiesNotes.thirteen:
                {
                    int randIndex =
                        Random
                            .Range(0,
                            CoreGameElements.i.allNotes.thirteenNotes.Length);
                    int randNote =
                        CoreGameElements.i.allNotes.thirteenNotes[randIndex];
                    return randNote;
                }
            case CoreGameElements.DifficutiesNotes.fourteen:
                {
                    int randIndex =
                        Random
                            .Range(0,
                            CoreGameElements.i.allNotes.fourteenNotes.Length);
                    int randNote =
                        CoreGameElements.i.allNotes.fourteenNotes[randIndex];
                    return randNote;
                }
            case CoreGameElements.DifficutiesNotes.fifteen:
                {
                    int randIndex =
                        Random
                            .Range(0,
                            CoreGameElements.i.allNotes.fifteenNotes.Length);
                    int randNote =
                        CoreGameElements.i.allNotes.fifteenNotes[randIndex];
                    return randNote;
                }
            case CoreGameElements.DifficutiesNotes.sixteen:
                {
                    int randIndex =
                        Random
                            .Range(0,
                            CoreGameElements.i.allNotes.sixteenNotes.Length);
                    int randNote =
                        CoreGameElements.i.allNotes.sixteenNotes[randIndex];
                    return randNote;
                }
            case CoreGameElements.DifficutiesNotes.seventeen:
                {
                    int randIndex =
                        Random
                            .Range(0,
                            CoreGameElements.i.allNotes.seventeenNotes.Length);
                    int randNote =
                        CoreGameElements.i.allNotes.seventeenNotes[randIndex];
                    return randNote;
                }
            default:
                return 0;
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
