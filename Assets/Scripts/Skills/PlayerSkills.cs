using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class PlayerSkills
{
    public delegate void SkilledUnlocked(SkillType skillType);

    public static event SkilledUnlocked onSkillUnlocked;

    [System.Serializable]
    public enum SkillType
    {
        none,
        sprint_1,
        sprint_2,
        sprint_3,
        health_1,
        health_2,
        health_3,
        readerRadius_1,
        readerRadius_2,
        smashRadius_1,
        smashRadius_2,
        sprintDuration_1,
        sprintDuration_2,
        addedStreakTime_1,
        addedStreakTime_2,
        addedStreakTime_3,
        initialStreakTime_1,
        initialStreakTime_2,
        initialStreakTime_3
    }

    public static List<SkillType> unlockedSkills = new List<SkillType>();

    private static int skillPoints;

    //this will be called in 'cleanup'
    [RuntimeInitializeOnLoadMethod]
    private static void Initialise()
    {
        skillPoints = 0;
    }

    public static void AddSkillPoint()
    {
        skillPoints++;
        UIController
            .UpdateTextUI(UIController.UITextComponents.skillPointText,
            skillPoints.ToString());
        UIController
            .UpdateTextUI(UIController.UITextComponents.skillMenuSkillPointText,
            skillPoints.ToString());
    }

    public static void UnlockSkill(SkillType skillType)
    {
        if (!IsSkillUnlocked(skillType))
        {
            unlockedSkills.Add (skillType);
            onSkillUnlocked (skillType);
        }
    }

    public static bool IsSkillUnlocked(SkillType skillType)
    {
        return unlockedSkills.Contains(skillType);
    }

    public static SkillType GetSkillRequirement(SkillType skillType)
    {
        if (GetSkillType(skillType).requiresPriorSkill)
        {
            return GetSkillType(skillType).requiredSkill;
        }
        else
            return SkillType.none;
    }

    public static CoreSkills.Skill GetSkillType(SkillType skillType)
    {
        foreach (CoreSkills.Skill skill in CoreSkills.i.skills)
        {
            if (skill.skillType == skillType) return skill;
        }
        return null;
    }

    public static bool TryUnlockSkill(SkillType skillType)
    {
        SkillType skillRequirement = GetSkillRequirement(skillType);

        if (skillRequirement != SkillType.none)
        {
            if (IsSkillUnlocked(skillRequirement))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    public static void RemoveSkillPoint()
    {
        skillPoints--;
    }

    public static bool
    CanUnlock(SkillType skillType, RectTransform toolTipSpawn)
    {
        int skillPointsRequired = GetSkillType(skillType).skillPointsRequired;
        if (IsSkillUnlocked(skillType))
        {
            Tooltip
                .SetToolTip_Static("Skill Already Unlocked!",
                toolTipSpawn.localPosition,
                toolTipSpawn.root);
            SoundController.PlaySound(SoundController.Sound.IncorectNote);
            return false;
        }
        else
        {
            if (TryUnlockSkill(skillType))
            {
                if (skillPoints >= skillPointsRequired)
                {
                    skillPoints -= skillPointsRequired;
                    UIController
                        .UpdateTextUI(UIController
                            .UITextComponents
                            .skillPointText,
                        skillPoints.ToString());
                    UIController
                        .UpdateTextUI(UIController
                            .UITextComponents
                            .skillMenuSkillPointText,
                        skillPoints.ToString());
                    Tooltip
                        .SetToolTip_Static("Skill Unlocked: " +
                        GetSkillType(skillType).skillName +
                        "\n",
                        toolTipSpawn.localPosition,
                        toolTipSpawn.root);
                    UnlockSkill (skillType);
                    return true;
                }
                else
                {
                    Tooltip
                        .SetToolTip_Static("Not Enough Skill Points!",
                        toolTipSpawn.localPosition,
                        toolTipSpawn.root);
                    SoundController
                        .PlaySound(SoundController.Sound.IncorectNote);
                    return false;
                }
            }
            else
            {
                var skillRequired = GetSkillType(skillType).requiredSkill;
                Tooltip
                    .SetToolTip_Static("Requires Skill: " +
                    GetSkillType(skillRequired).skillName,
                    toolTipSpawn.localPosition,
                    toolTipSpawn.root);
                SoundController.PlaySound(SoundController.Sound.IncorectNote);
                return false;
            }
        }
    }

    public static void SaveAllSkills()
    {
        // foreach (SkillType skill in unlockedSkills)
        // {
        //     CoreGameElements.i.gameSave.savedUnlockedSkills.Add (skill);
        // }
        CoreGameElements.i.gameSave.savedUnlockedSkills =
            DeepCopy(unlockedSkills);
    }

    public static void LoadAllSkills()
    {
        // var savedUnlockedSkills =
        //     CoreGameElements.i.gameSave.savedUnlockedSkills;
        // foreach (SkillType skill in savedUnlockedSkills)
        // {
        //     unlockedSkills.Add (skill);
        // }
        unlockedSkills =
            DeepCopy(CoreGameElements.i.gameSave.savedUnlockedSkills);
        UIController.UpdateSkillButtons();
    }

    public static T DeepCopy<T>(T item)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream stream = new MemoryStream();
        formatter.Serialize (stream, item);
        stream.Seek(0, SeekOrigin.Begin);
        T result = (T) formatter.Deserialize(stream);
        stream.Close();
        return result;
    }
}
