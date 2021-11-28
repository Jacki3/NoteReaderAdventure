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
        }

        if (currentHealth > 0)
        {
            //play sound
            //animate
            UIController
                .UpdateSliderAmount(UIController.UIImageComponents.healthBar,
                maxHealth,
                currentHealth);
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
        UIController
            .UpdateSliderAmount(UIController.UIImageComponents.healthBar,
            maxHealth,
            currentHealth);
        //sound
        //animate
    }

    public static void UpgradeHealthBar(int heatlhUpgraded)
    {
        maxHealth += heatlhUpgraded;
        currentHealth = maxHealth;
        //update UI?
        //animate
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
        //updateUI
        //animate
        //sound
    }

    public static void ResetHealth()
    {
        currentHealth = maxHealth;
        UIController
            .UpdateSliderAmount(UIController.UIImageComponents.healthBar,
            maxHealth,
            currentHealth);
    }

    public static bool NotMaxHealth() => currentHealth < maxHealth;

    public static bool CriticalHealth() =>
        currentHealth <= criticalHealthAmount;

    public static bool LowHealth() => currentHealth <= lowHealthAmount;

    public static bool HasShield() => hasShield;

    public static bool HasProtectiveShield() => strongShield;
}
