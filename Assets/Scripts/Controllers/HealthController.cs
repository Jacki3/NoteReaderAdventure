using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HealthController
{
    private static int maxHealth;

    private static int currentHealth = CoreGameElements.i.maxHealth;

    private static int criticalHealthAmount;

    private static int lowHealthAmount;

    private static bool hasShield = false;

    private static bool strongShield = false;

    public static void SetHealth(int health)
    {
        if (CoreGameElements.i != null)
        {
            maxHealth = CoreGameElements.i.maxHealth;
            criticalHealthAmount = CoreGameElements.i.criticalHealth;
            lowHealthAmount = CoreGameElements.i.lowHealth;
        }
        PlayerSkills.onSkillUnlocked += UpdateMaxHealth;
        int healthDiff = CoreGameElements.i.maxHealth - health;
        RemoveHealth(healthDiff, false);
    }

    private static void UpdateMaxHealth(PlayerSkills.SkillType skillType)
    {
        switch (skillType)
        {
            case PlayerSkills.SkillType.health_1:
                UpgradeMaxHealth(4);
                break;
            case PlayerSkills.SkillType.health_2:
                UpgradeMaxHealth(4);
                break;
            case PlayerSkills.SkillType.health_3:
                UpgradeMaxHealth(4);
                break;
        }
    }

    public static void RemoveHealth(int healthRemoved, bool animate)
    {
        if (strongShield)
        {
            strongShield = false;
            SpriteController.SetSprite(SpriteController.Sprites.shield);
            //bubble pop sound
        }
        else if (hasShield)
        {
            hasShield = false;
            SpriteController.SetSprite(SpriteController.Sprites.noShield);
            //bubble pop sound
        }
        else
        {
            currentHealth -= healthRemoved;
            UIController.UpdateHearts(healthRemoved, true);
            if (animate)
            {
                EZCameraShake
                    .CameraShaker
                    .Instance
                    .ShakeOnce(.35f, 1f, .5f, 1f);
                SoundController.PlaySound(SoundController.Sound.PlayerHurt);
                FXController
                    .SetAnimatorTrigger_Static(FXController
                        .Animations
                        .PlayerAnimator,
                    "TakenDamage");
            }
        }

        if (currentHealth <= 0)
        {
            LivesController.RemoveLife();
            SoundController.PlaySound(SoundController.Sound.PlayerDeath);
        }
        CoreGameElements.i.gameSave.playerHealth = currentHealth;
    }

    public static void AddHealth(int healthAdded)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += healthAdded;
            UIController.UpdateHearts(healthAdded, false);

            FXController
                .LerpSlider_Static(currentHealth,
                currentHealth - healthAdded,
                .5f,
                maxHealth,
                UIController.UIImageComponents.healthBar);
        }
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        FXController
            .SetAnimatorTrigger_Static(FXController.Animations.PlayerAnimator,
            "HealthGained");
        //sound
        //animate
    }

    public static void UpgradeMaxHealth(int heatlhUpgraded)
    {
        maxHealth += heatlhUpgraded;
        currentHealth = maxHealth;

        UIController.UpdateHearts(maxHealth, false);
        UIController.AddHeart();

        UpgradeHealthBar();
    }

    public static void UpgradeHealthBar()
    {
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
