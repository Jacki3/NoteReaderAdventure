using UnityEngine;

public class SkillEnumButton : MonoBehaviour
{
    public PlayerSkills.SkillType skillType;

    public CoreUIElements.ImageUI

            iconImage,
            skillLine,
            backGround;

    public void UnlockSkill(SkillEnumButton enumScript)
    {
        if (PlayerSkills.CanUnlock(enumScript.skillType, backGround))
        {
            UIController.UpdateImageColour(iconImage, Color.white);
            UIController.UpdateImageColour(skillLine, Color.white);
            UIController.UpdateImageColour(backGround, Color.white);
        }
    }
}
