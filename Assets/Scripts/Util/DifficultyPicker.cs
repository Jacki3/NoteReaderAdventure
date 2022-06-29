using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        var easyLevelRange = new Count();
        var medLevelRange = new Count();
        var hardLevelRange = new Count();
        var hardestLevelRange = new Count();

        easyLevelRange.minimum = 0;
        easyLevelRange.maximum = Mathf.RoundToInt(diff);

        medLevelRange.minimum = easyLevelRange.maximum;
        medLevelRange.maximum = easyLevelRange.maximum + Mathf.RoundToInt(diff);

        hardLevelRange.minimum = medLevelRange.maximum;
        hardLevelRange.maximum = medLevelRange.maximum + Mathf.RoundToInt(diff);

        hardestLevelRange.minimum = hardLevelRange.maximum;
        hardestLevelRange.maximum = CoreGameElements.i.totalLevels;

        if (IsWithinRange(easyLevelRange.minimum, easyLevelRange.maximum, level)
        )
        {
            CoreGameElements.i.currentDifficulty =
                CoreGameElements.Difficuties.easy;
        }
        else if (
            IsWithinRange(medLevelRange.minimum, medLevelRange.maximum, level)
        )
        {
            CoreGameElements.i.currentDifficulty =
                CoreGameElements.Difficuties.medium;
        }
        else if (
            IsWithinRange(hardLevelRange.minimum, hardLevelRange.maximum, level)
        )
        {
            CoreGameElements.i.currentDifficulty =
                CoreGameElements.Difficuties.hard;
        }
        else if (
            IsWithinRange(hardestLevelRange.minimum,
            hardestLevelRange.maximum,
            level)
        )
        {
            CoreGameElements.i.currentDifficulty =
                CoreGameElements.Difficuties.hardest;
        }
    }

    private static bool IsWithinRange(int min, int max, int value)
    {
        return value < max && value > min;
    }
}
