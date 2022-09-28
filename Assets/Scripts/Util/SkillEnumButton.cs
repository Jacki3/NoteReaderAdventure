using UnityEngine;
using UnityEngine.EventSystems;

public class SkillEnumButton : MonoBehaviour, ISelectHandler
{
    public PlayerSkills.SkillType skillType;

    public RectTransform toolTipSpawn;

    public Color skillLineColour;

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
        //IF NOT SKIP BUTTON
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

    public void Skip()
    {
        PlayerSkills.RemoveSkillPoint();
        pauseMenu.HideSkillMenu();
    }

    private void UpdateUI(bool isUnlocked)
    {
        Color32 skillColour = isUnlocked ? skillLineColour : Color.grey;

        UIController.UpdateImageColour (skillLine, skillColour);
        UIController.UpdateImageColour (backGround, skillColour);
    }
}
