using UnityEngine;
using UnityEngine.EventSystems;

public class SkillEnumButton : MonoBehaviour, ISelectHandler
{
    public PlayerSkills.SkillType skillType;

    public RectTransform toolTipSpawn;

    public CoreUIElements.ImageUI

            iconImage,
            skillLine,
            backGround;

    public void OnSelect(BaseEventData eventData)
    {
        var currentSkill = PlayerSkills.GetSkillType(skillType);
        Tooltip
            .SetToolTip_Static(currentSkill.skillName +
            "\n" +
            currentSkill.skillDescription +
            "\n" +
            "Skill Points Required: " +
            currentSkill.skillPointsRequired,
            toolTipSpawn.localPosition,
            transform.root);

        //launch tooltip and show info after being selected for x seconds
    }

    public void UnlockSkill(SkillEnumButton enumScript)
    {
        if (PlayerSkills.CanUnlock(enumScript.skillType, toolTipSpawn))
        {
            UIController.UpdateImageColour(iconImage, Color.white);
            UIController.UpdateImageColour(skillLine, Color.white);
            UIController.UpdateImageColour(backGround, Color.white);
        }
    }
}
