using System.Collections;
using UnityEngine;

public static class ExperienceController
{
    private static int level = 0;

    private static int currentXP;

    private static int XPToNextLvl;

    [RuntimeInitializeOnLoadMethod]
    static void SetInitialLevel()
    {
        SetLevel(1);
    }

    public static bool AddXP(int XPToAdd)
    {
        currentXP += XPToAdd;

        UIController
            .UpdateSliderAmount(UIController.UIImageComponents.XPBar,
            XPToNextLvl,
            currentXP);

        if (currentXP >= XPToNextLvl)
        {
            SetLevel(level + 1);
            return true;
        }

        ScoreDisplayUpdater
            .StartRoutine(currentXP,
            UIController.UITextComponents.currentXPText);

        return false;
    }

    private static void SetLevel(int value)
    {
        level = value;
        currentXP = currentXP - XPToNextLvl;
        if (level > 1) PlayerSkills.AddSkillPoint();

        //look into this math -- seems to be an error that lvl 1 and 2 both require 100 XP?
        XPToNextLvl =
            (int)(50f * (Mathf.Pow(level + 1, 2) - (5 * (level + 1)) + 8));

        UIController
            .UpdateTextUI(UIController.UITextComponents.currentXPText,
            currentXP.ToString());
        UIController
            .UpdateTextUI(UIController.UITextComponents.currentLvlText,
            level.ToString());
        UIController
            .UpdateTextUI(UIController.UITextComponents.XPToNextLvlText,
            XPToNextLvl.ToString());
        UIController
            .UpdateSliderAmount(UIController.UIImageComponents.XPBar,
            XPToNextLvl,
            currentXP);

        //animation controller updated level up
    }

    public static bool GetLevelRequirement(int levelRequired)
    {
        if (level >= levelRequired)
            return true;
        else
            return false;
    }
}
