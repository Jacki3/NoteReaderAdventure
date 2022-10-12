using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public static class ExperienceController
{
    public delegate void LevelUpHandler();

    public static event LevelUpHandler LevelUp;

    private static int level = 0;

    private static int currentXP;

    private static int XPToNextLvl;

    public static void SetInitialLevel()
    {
        SetLevel(1);
    }

    public static void SetXP()
    {
        currentXP = CoreGameElements.i.gameSave.XP;
        XPToNextLvl = CoreGameElements.i.gameSave.XPToNextLvl;

        int savedLevel = CoreGameElements.i.gameSave.currentLevel;
        level = savedLevel;
        if (level >= 1)
            UIController
                .UpdateTextUI(UIController.UITextComponents.currentLvlText,
                level.ToString());
        UIController
            .UpdateSliderAmount(UIController.UIImageComponents.XPBar,
            XPToNextLvl,
            currentXP);
    }

    public static bool AddXP(int XPToAdd)
    {
        currentXP += XPToAdd;

        FXController
            .LerpSlider_Static(currentXP,
            currentXP - XPToAdd,
            1,
            XPToNextLvl,
            UIController.UIImageComponents.XPBar);

        if (currentXP >= XPToNextLvl)
        {
            SetLevel(level + 1);
            FXController
                .LerpSlider_Static(currentXP,
                currentXP - XPToAdd,
                1,
                XPToNextLvl,
                UIController.UIImageComponents.XPBar);
            return true;
        }

        UIController
            .UpdateTextUI(UIController.UITextComponents.currentXPText,
            currentXP.ToString());

        // ScoreDisplayUpdater
        //     .StartRoutine(currentXP,
        //     UIController.UITextComponents.currentXPText);
        return false;
    }

    private static void SetLevel(int value)
    {
        level = value;
        currentXP = currentXP - XPToNextLvl;

        if (level > 1)
        {
            PlayerSkills.AddSkillPoint();

            // FXController.SpawnEffect_Static(FXController.Effects.PlayerLevelUp);
            HealthController.ResetHealth();
            SoundController.PlaySound(SoundController.Sound.PlayerLvlUp);
            LevelUp();
        }

        XPToNextLvl =
            (int)(50f * (Mathf.Pow(level + 1.1f, 2) - (5 * (level + 1)) + 8));

        UIController
            .UpdateTextUI(UIController.UITextComponents.currentXPText,
            currentXP.ToString());
        UIController
            .UpdateTextUI(UIController.UITextComponents.currentLvlText,
            level.ToString());
        UIController
            .UpdateTextUI(UIController.UITextComponents.XPToNextLvlText,
            XPToNextLvl.ToString());

        if (level == 1)
        {
            CoreGameElements.i.gameSave.XP = currentXP;
            CoreGameElements.i.gameSave.XPToNextLvl = XPToNextLvl;
            CoreGameElements.i.gameSave.currentLevel = level;
        }
    }

    public static bool GetLevelRequirement(int levelRequired)
    {
        if (level >= levelRequired)
            return true;
        else
            return false;
    }

    public static void SaveXP()
    {
        CoreGameElements.i.gameSave.XP = currentXP;
        CoreGameElements.i.gameSave.XPToNextLvl = XPToNextLvl;
        CoreGameElements.i.gameSave.currentLevel = level;
    }

    public static int GetLevel() => level;
}
