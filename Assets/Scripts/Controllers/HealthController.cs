using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HealthController
{
    private static int maxHealth;

    private static int currentHealth;

    private static int criticalHealthAmount;

    private static int lowHealthAmount;

    private static bool hasShield = false;

    private static bool strongShield = false;

    //this should be replaced by a controller which handles 'global/core things' such as health, score, XP etc. OR be a monobehaviour!
    [RuntimeInitializeOnLoadMethod]
    private static void SetHealth()
    {
        if (CoreGameElements.i != null)
        {
            maxHealth = CoreGameElements.i.maxHealth;
            criticalHealthAmount = CoreGameElements.i.criticalHealth;
            lowHealthAmount = CoreGameElements.i.lowHealth;
        }
        currentHealth = maxHealth;
        PlayerSkills.onSkillUnlocked += UpdateMaxHealth;
    }

    private static void UpdateMaxHealth(PlayerSkills.SkillType skillType)
    {
        switch (skillType)
        {
            case PlayerSkills.SkillType.health_1:
                UpgradeHealthBar(3);
                break;
            case PlayerSkills.SkillType.health_2:
                UpgradeHealthBar(5);
                break;
            case PlayerSkills.SkillType.health_3:
                UpgradeHealthBar(10);
                break;
        }
    }

    public static void RemoveHealth(int healthRemoved)
    {
        if (strongShield)
        {
            strongShield = false;
        }
        else if (hasShield)
        {
            hasShield = false;
        }
        else
        {
            currentHealth -= healthRemoved;
            EZCameraShake.CameraShaker.Instance.ShakeOnce(.35f, 1f, .5f, 1f);
            SoundController.PlaySound(SoundController.Sound.PlayerHurt);
        }

        if (currentHealth > 0)
        {
            FXController
                .LerpSlider_Static(currentHealth,
                currentHealth + healthRemoved,
                .5f,
                maxHealth,
                UIController.UIImageComponents.healthBar);
            FXController
                .SetAnimatorTrigger_Static(FXController
                    .Animations
                    .PlayerAnimator,
                "TakenDamage");
            //play sound
        }
        else
        {
            LivesController.RemoveLife();
        }
    }

    public static void AddHealth(int healthAdded)
    {
        if (currentHealth < maxHealth) currentHealth += healthAdded;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        FXController
            .LerpSlider_Static(currentHealth,
            currentHealth - healthAdded,
            .5f,
            maxHealth,
            UIController.UIImageComponents.healthBar);

        FXController
            .SetAnimatorTrigger_Static(FXController.Animations.PlayerAnimator,
            "HealthGained");
        //sound
        //animate
    }

    public static void UpgradeHealthBar(int heatlhUpgraded)
    {
        maxHealth += heatlhUpgraded;
        currentHealth = maxHealth;
        float currentXPBarX =
            CoreUIElements
                .i
                .GetSliderComponent(UIController.UIImageComponents.healthBar)
                .transform
                .localScale
                .x;
        float newBarX = currentXPBarX += .3f;
        FXController
            .ExpandSlider(newBarX,
            .5f,
            UIController.UIImageComponents.healthBar);
        //animate like level up
        //sound
    }

    public static void AddShield(bool _strongShield)
    {
        if (_strongShield)
        {
            strongShield = true;
            hasShield = true;
        }
        else
        {
            hasShield = true;
        }
        //sound
    }

    public static void ResetHealth()
    {
        currentHealth = maxHealth;

        FXController
            .LerpSlider_Static(maxHealth,
            currentHealth,
            1,
            maxHealth,
            UIController.UIImageComponents.healthBar);
    }

    public static bool NotMaxHealth() => currentHealth < maxHealth;

    public static bool CriticalHealth() =>
        currentHealth <= criticalHealthAmount;

    public static bool LowHealth() => currentHealth <= lowHealthAmount;

    public static bool HasShield() => hasShield;

    public static bool HasProtectiveShield() => strongShield;
}
