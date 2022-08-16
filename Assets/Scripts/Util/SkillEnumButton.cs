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

        UpdateSkillUI();
    }

    public void UpdateSkillUI()
    {
        var unlockedSkills = CoreGameElements.i.gameSave.savedUnlockedSkills;

        if (unlockedSkills.Count > 0)
        {
            foreach (PlayerSkills.SkillType _skillType in unlockedSkills)
            {
                if (skillType == _skillType) UpdateUI(true);
            }
        }
        else
            UpdateUI(false);
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
            UpdateUI(true);
            pauseMenu.HideSkillMenu();
        }
    }

    private void UpdateUI(bool isUnlocked)
    {
        Color32 skillColour = isUnlocked ? Color.white : Color.grey;

        UIController.UpdateImageColour (iconImage, skillColour);
        UIController.UpdateImageColour (skillLine, skillColour);
        UIController.UpdateImageColour (backGround, skillColour);
    }
}
