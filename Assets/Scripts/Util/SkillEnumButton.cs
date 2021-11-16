using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEnumButton : MonoBehaviour
{
    public PlayerSkills.SkillType skillType;

    public void UnlockSkill(SkillEnumButton enumScript)
    {
        PlayerSkills.CanUnlock(enumScript.skillType);
    }
}
