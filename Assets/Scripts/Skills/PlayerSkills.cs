using System.Collections.Generic;
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
        smashRadius_2

        //magic? should this be separate via statue?
        //health? ""
    }

    private static List<SkillType> unlockedSkills = new List<SkillType>();

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

    public static bool
    CanUnlock(SkillType skillType, CoreUIElements.ImageUI buttonImage)
    {
        int skillPointsRequired = GetSkillType(skillType).skillPointsRequired;
        if (IsSkillUnlocked(skillType))
        {
            Debug.Log("skill already unlocked!");
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
                    Debug.Log("skill unlocked: " + skillType);
                    Debug.Log("Skill points remaining: " + skillPoints);
                    UnlockSkill (skillType);
                    return true;
                }
                else
                {
                    Tooltip
                        .SetToolTip_Static("not enough skill points!",
                        buttonImage.image.transform.root.position +
                        buttonImage.image.transform.localPosition);
                    Debug.Log("not enough skills");
                    return false;
                }
            }
            else
            {
                Debug.Log("requires skill: " + GetSkillRequirement(skillType));
                return false;
            }
        }
    }
}
