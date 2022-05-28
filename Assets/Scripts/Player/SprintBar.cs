using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintBar : MonoBehaviour
{
    public float maxStamina;

    public int sprintDecreaseAmount = 2;

    public static bool canSprint;

    private float currentStamina;

    private static bool isSprinting;

    private static GameObject staminaBar;

    private void Awake()
    {
        staminaBar = transform.GetChild(0).gameObject;
        PlayerSkills.onSkillUnlocked += IncreaseStaminaBar;
    }

    private void IncreaseStaminaBar(PlayerSkills.SkillType skillType)
    {
        switch (skillType)
        {
            case PlayerSkills.SkillType.sprintDuration_1:
                UpgradeStaminaBar(3);
                break;
            case PlayerSkills.SkillType.sprintDuration_2:
                UpgradeStaminaBar(6);
                break;
        }
    }

    public void UpgradeStaminaBar(int staminaToAdd)
    {
        maxStamina += staminaToAdd;
        currentStamina = maxStamina;
        float currentStaminaBar =
            CoreUIElements
                .i
                .GetSliderComponent(UIController.UIImageComponents.staminaBar)
                .transform
                .localScale
                .x;
        float newBarX = currentStaminaBar += .3f;
        FXController
            .ExpandSlider(newBarX,
            .5f,
            UIController.UIImageComponents.staminaBar);
        //animate like level up
        //sound
    }

    void Update()
    {
        if (isSprinting)
        {
            currentStamina -= Time.deltaTime * sprintDecreaseAmount;
        }
        else if (currentStamina < maxStamina)
        {
            currentStamina += Time.deltaTime * sprintDecreaseAmount;
        }

        if (currentStamina < 0) currentStamina = 0;

        FXController
            .LerpSlider_Static(currentStamina,
            currentStamina,
            .5f,
            maxStamina,
            UIController.UIImageComponents.staminaBar);

        canSprint = currentStamina <= 0 ? false : true;
    }

    public static void StartSprinting() => isSprinting = true;

    public static void StopSprinting() => isSprinting = false;

    public static void ShowStaminaBar() => staminaBar.SetActive(true);
}
