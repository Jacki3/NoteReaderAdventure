using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public static class DifficultyPicker
{
    public class Count
    {
        public int minimum;

        public int maximum;
    }

    public static void ChooseDifficultyOnLevel(int level)
    {
        if (CoreGameElements.i.difficultyChangeLevel == 0)
            Debug.LogError("Difficulty Change Level is 0!");
        float difficultyDivider =
            CoreGameElements.i.totalLevels /
            CoreGameElements.i.difficultyChangeLevel;
        float diff = CoreGameElements.i.totalLevels / difficultyDivider;

        var beginnerRange = new Count();
        var veryEasyRange = new Count();
        var easyRange = new Count();
        var medRange = new Count();
        var intermeidateRange = new Count();
        var hardRange = new Count();
        var veryHardrange = new Count();
        var ultraHardRange = new Count();
        var superHardRange = new Count();
        var hardestRange = new Count();

        beginnerRange.minimum = 0;
        beginnerRange.maximum = Mathf.RoundToInt(diff);

        veryEasyRange.minimum = beginnerRange.maximum;
        veryEasyRange.maximum = beginnerRange.maximum + Mathf.RoundToInt(diff);

        easyRange.minimum = veryEasyRange.maximum;
        easyRange.maximum = veryEasyRange.maximum + Mathf.RoundToInt(diff);

        medRange.minimum = easyRange.maximum;
        medRange.maximum = easyRange.maximum + Mathf.RoundToInt(diff);

        intermeidateRange.minimum = medRange.maximum;
        intermeidateRange.maximum = medRange.maximum + Mathf.RoundToInt(diff);

        hardRange.minimum = intermeidateRange.maximum;
        hardRange.maximum = intermeidateRange.maximum + Mathf.RoundToInt(diff);

        veryHardrange.minimum = hardRange.maximum;
        veryHardrange.maximum = hardRange.maximum + Mathf.RoundToInt(diff);

        ultraHardRange.minimum = veryHardrange.maximum;
        ultraHardRange.maximum = veryHardrange.maximum + Mathf.RoundToInt(diff);

        superHardRange.minimum = ultraHardRange.maximum;
        superHardRange.maximum =
            ultraHardRange.maximum + Mathf.RoundToInt(diff);

        hardestRange.minimum = superHardRange.maximum;
        hardestRange.maximum = CoreGameElements.i.totalLevels + 1;

        if (!CoreGameElements.i.arenaMode)
        {
            if (
                IsWithinRange(beginnerRange.minimum,
                beginnerRange.maximum,
                level)
            )
            {
                CoreGameElements.i.currentDifficulty =
                    CoreGameElements.Difficuties.absoluteBeginner;
                AudioController.helmClock.bpm = 90;
            }
            else if (
                IsWithinRange(veryEasyRange.minimum,
                veryEasyRange.maximum,
                level)
            )
            {
                CoreGameElements.i.currentDifficulty =
                    CoreGameElements.Difficuties.veryEasy;
                AudioController.helmClock.bpm = 90;
            }
            else if (IsWithinRange(easyRange.minimum, easyRange.maximum, level))
            {
                CoreGameElements.i.currentDifficulty =
                    CoreGameElements.Difficuties.easy;
                AudioController.helmClock.bpm = 90;
            }
            else if (IsWithinRange(medRange.minimum, medRange.maximum, level))
            {
                CoreGameElements.i.currentDifficulty =
                    CoreGameElements.Difficuties.medium;
                AudioController.helmClock.bpm = 90;
            }
            else if (
                IsWithinRange(intermeidateRange.minimum,
                intermeidateRange.maximum,
                level)
            )
            {
                CoreGameElements.i.currentDifficulty =
                    CoreGameElements.Difficuties.intermediate;
                AudioController.helmClock.bpm = 90;
            }
            else if (IsWithinRange(hardRange.minimum, hardRange.maximum, level))
            {
                CoreGameElements.i.currentDifficulty =
                    CoreGameElements.Difficuties.hard;
                AudioController.helmClock.bpm = 90;
            }
            else if (
                IsWithinRange(veryHardrange.minimum,
                veryHardrange.maximum,
                level)
            )
            {
                CoreGameElements.i.currentDifficulty =
                    CoreGameElements.Difficuties.veryHard;
                AudioController.helmClock.bpm = 95;
            }
            else if (
                IsWithinRange(ultraHardRange.minimum,
                ultraHardRange.maximum,
                level)
            )
            {
                CoreGameElements.i.currentDifficulty =
                    CoreGameElements.Difficuties.ultraHard;
                AudioController.helmClock.bpm = 95;
            }
            else if (
                IsWithinRange(superHardRange.minimum,
                superHardRange.maximum,
                level)
            )
            {
                CoreGameElements.i.currentDifficulty =
                    CoreGameElements.Difficuties.superHard;
                AudioController.helmClock.bpm = 100;
            }
            else if (
                IsWithinRange(hardestRange.minimum, hardestRange.maximum, level)
            )
            {
                CoreGameElements.i.currentDifficulty =
                    CoreGameElements.Difficuties.hardest;
                AudioController.helmClock.bpm = 105;
            }
        }
        else
        {
            CoreGameElements.i.currentDifficulty =
                CoreGameElements.Difficuties.hardest;
            AudioController.helmClock.bpm = 100;
        }
    }

    public static void ChooseDifficultyOnLevelNotes(int level)
    {
        if (CoreGameElements.i.difficultyChangeLevel == 0)
            Debug.LogError("Difficulty Change Level is 0!");
        float difficultyDivider =
            CoreGameElements.i.totalLevels /
            CoreGameElements.i.difficultyChangeLevelNotes;
        float diff = CoreGameElements.i.totalLevels / difficultyDivider;

        var one = new Count();
        var two = new Count();
        var three = new Count();
        var four = new Count();
        var five = new Count();
        var six = new Count();
        var seven = new Count();
        var eight = new Count();
        var nine = new Count();
        var ten = new Count();
        var eleven = new Count();
        var twelve = new Count();
        var thirteen = new Count();
        var fourteen = new Count();
        var fifteen = new Count();
        var sixteen = new Count();
        var seventeen = new Count();

        one.minimum = 0;
        one.maximum = Mathf.RoundToInt(diff);

        two.minimum = one.maximum;
        two.maximum = one.maximum + Mathf.RoundToInt(diff);

        three.minimum = two.maximum;
        three.maximum = two.maximum + Mathf.RoundToInt(diff);

        four.minimum = three.maximum;
        four.maximum = three.maximum + Mathf.RoundToInt(diff);

        five.minimum = four.maximum;
        five.maximum = four.maximum + Mathf.RoundToInt(diff);

        six.minimum = five.maximum;
        six.maximum = five.maximum + Mathf.RoundToInt(diff);

        seven.minimum = six.maximum;
        seven.maximum = six.maximum + Mathf.RoundToInt(diff);

        eight.minimum = seven.maximum;
        eight.maximum = seven.maximum + Mathf.RoundToInt(diff);

        nine.minimum = eight.maximum;
        nine.maximum = eight.maximum + Mathf.RoundToInt(diff);

        ten.minimum = nine.maximum;
        ten.maximum = nine.maximum + Mathf.RoundToInt(diff);

        eleven.minimum = ten.maximum;
        eleven.maximum = ten.maximum + Mathf.RoundToInt(diff);

        twelve.minimum = eleven.maximum;
        twelve.maximum = eleven.maximum + Mathf.RoundToInt(diff);

        thirteen.minimum = twelve.maximum;
        thirteen.maximum = twelve.maximum + Mathf.RoundToInt(diff);

        fourteen.minimum = thirteen.maximum;
        fourteen.maximum = thirteen.maximum + Mathf.RoundToInt(diff);

        fifteen.minimum = fourteen.maximum;
        fifteen.maximum = fourteen.maximum + Mathf.RoundToInt(diff);

        sixteen.minimum = fifteen.maximum;
        sixteen.maximum = fifteen.maximum + Mathf.RoundToInt(diff);

        seventeen.minimum = sixteen.maximum;
        seventeen.maximum = CoreGameElements.i.totalLevels + 1;

        if (!CoreGameElements.i.arenaMode)
        {
            if (IsWithinRange(one.minimum, one.maximum, level))
            {
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.one;
            }
            else if (IsWithinRange(two.minimum, two.maximum, level))
            {
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.two;
            }
            else if (IsWithinRange(three.minimum, three.maximum, level))
            {
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.three;
            }
            else if (IsWithinRange(four.minimum, four.maximum, level))
            {
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.four;
            }
            else if (IsWithinRange(five.minimum, five.maximum, level))
            {
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.five;
            }
            else if (IsWithinRange(six.minimum, six.maximum, level))
            {
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.six;
            }
            else if (IsWithinRange(seven.minimum, seven.maximum, level))
            {
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.seven;
            }
            else if (IsWithinRange(eight.minimum, eight.maximum, level))
            {
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.eight;
            }
            else if (IsWithinRange(nine.minimum, nine.maximum, level))
            {
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.nine;
            }
            else if (IsWithinRange(ten.minimum, ten.maximum, level))
            {
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.ten;
            }
            else if (IsWithinRange(eleven.minimum, eleven.maximum, level))
            {
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.eleven;
            }
            else if (IsWithinRange(twelve.minimum, twelve.maximum, level))
            {
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.twelve;
            }
            else if (IsWithinRange(thirteen.minimum, thirteen.maximum, level))
            {
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.thirteen;
            }
            else if (IsWithinRange(fourteen.minimum, fourteen.maximum, level))
            {
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.fourteen;
            }
            else if (IsWithinRange(fifteen.minimum, fifteen.maximum, level))
            {
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.fifteen;
            }
            else if (IsWithinRange(sixteen.minimum, sixteen.maximum, level))
            {
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.sixteen;
            }
            else if (IsWithinRange(seventeen.minimum, seventeen.maximum, level))
            {
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.seventeen;
            }
        }
        else
        {
            CoreGameElements.i.currentDifficultyNotes =
                CoreGameElements.DifficutiesNotes.one;
        }
    }

    private static bool IsWithinRange(int min, int max, int value)
    {
        return value < max && value > min;
    }

    public static void SetArenaDifficulty(int wave)
    {
        switch (wave)
        {
            case 0:
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.one;
                break;
            case 1:
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.two;
                break;
            case 2:
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.three;
                break;
            case 3:
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.four;
                break;
            case 4:
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.five;
                break;
            case 5:
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.six;
                break;
            case 6:
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.seven;
                break;
            case 7:
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.eight;
                break;
            case 8:
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.nine;
                break;
            case 9:
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.ten;
                break;
            case 10:
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.eleven;
                break;
            case 11:
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.twelve;
                break;
            case 12:
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.thirteen;
                break;
            case 13:
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.fourteen;
                break;
            case 14:
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.fifteen;
                break;
            case 15:
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.sixteen;
                break;
            case 16:
                CoreGameElements.i.currentDifficultyNotes =
                    CoreGameElements.DifficutiesNotes.seventeen;
                break;
        }
    }
}
