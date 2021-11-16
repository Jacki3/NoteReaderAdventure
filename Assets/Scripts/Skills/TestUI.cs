using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUI : MonoBehaviour
{
    public PlayerSkills.SkillType skillType;

    public void UpgradeSkill()
    {
        PlayerSkills.CanUnlock(PlayerSkills.SkillType.sprint_1);
    }

    public void UpgradeSkill2()
    {
        PlayerSkills.CanUnlock(PlayerSkills.SkillType.sprint_2);
    }

    public void TestButton(TestUI test)
    {
    }
}
