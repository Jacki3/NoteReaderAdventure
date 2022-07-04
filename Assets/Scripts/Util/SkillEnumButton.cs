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

    private PauseMenu pauseMenu;

    void Start()
    {
        pauseMenu = transform.root.GetComponent<PauseMenu>();

        var unlockedSkills = CoreGameElements.i.gameSave.savedUnlockedSkills;

        foreach (PlayerSkills.SkillType _skillType in unlockedSkills)
        {
            if (skillType == _skillType) UpdateUI();
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        ShowSkillInfo();
    }

    public void ShowSkillInfo()
    {
        var currentSkill = PlayerSkills.GetSkillType(skillType);
        Tooltip
            .SetToolTipSkill_Static(currentSkill.skillName +
            "\n\n" +
            currentSkill.skillDescription,
            toolTipSpawn.localPosition,
            transform.root);
    }

    public void UnlockSkill(SkillEnumButton enumScript)
    {
        if (PlayerSkills.CanUnlock(enumScript.skillType, toolTipSpawn))
        {
            UpdateUI();
            pauseMenu.HideSkillMenu();
        }
    }

    private void UpdateUI()
    {
        UIController.UpdateImageColour(iconImage, Color.white);
        UIController.UpdateImageColour(skillLine, Color.white);
        UIController.UpdateImageColour(backGround, Color.white);
    }
}
