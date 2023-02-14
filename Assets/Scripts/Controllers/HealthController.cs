using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public static class HealthController
{
    private static int maxHealth;

    private static int currentHealth = CoreGameElements.i.maxHealth;

    private static int criticalHealthAmount;

    private static int lowHealthAmount;

    private static bool hasShield = false;

    private static bool strongShield = false;

    public static void SetHealth()
    {
        if (CoreGameElements.i != null)
        {
            maxHealth = CoreGameElements.i.maxHealth;
            criticalHealthAmount = CoreGameElements.i.criticalHealth;
            lowHealthAmount = CoreGameElements.i.lowHealth;
        }
        PlayerSkills.onSkillUnlocked += UpdateMaxHealth;
    }

    public static void UpdateHealth()
    {
        UpdateShield();
        currentHealth = CoreGameElements.i.gameSave.playerHealth;
        int healthRemoved = maxHealth - currentHealth;
        bool lifeLowerThanMax = currentHealth < maxHealth ? true : false;
        if (lifeLowerThanMax)
        {
            UIController.UpdateHearts (healthRemoved, lifeLowerThanMax);
        }
        UIController.ResetHearts();
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
        UIController.AddHeart(false);

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

    private static void UpdateShield()
    {
        hasShield = CoreGameElements.i.gameSave.hasShield;
        strongShield = CoreGameElements.i.gameSave.hasStrongShield;

        if (hasShield || strongShield)
        {
            AddShield (strongShield);
            if (hasShield)
                SpriteController.SetSprite(SpriteController.Sprites.shield);
            if (strongShield)
                SpriteController
                    .SetSprite(SpriteController.Sprites.protectiveShield);
        }
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

        UIController.ResetHearts();
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

    public static void SaveHealth()
    {
        CoreGameElements.i.gameSave.playerHealth = currentHealth;
        CoreGameElements.i.gameSave.maxHealth = maxHealth;
        CoreGameElements.i.gameSave.hasShield = hasShield;
        CoreGameElements.i.gameSave.hasStrongShield = strongShield;
    }
}
