using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HealthController
{
    private static int maxHealth;

    private static int currentHealth;

    private static bool hasShield = false;

    //this should be replaced by a controller which handles 'global/core things' such as health, score, XP etc. OR be a monobehaviour!
    [RuntimeInitializeOnLoadMethod]
    private static void SetHealth()
    {
        maxHealth = CoreGameElements.i.maxHealth;
        currentHealth = maxHealth;
    }

    public static void RemoveHealth(int healthRemoved)
    {
        if (!hasShield)
            currentHealth -= healthRemoved;
        else
            hasShield = false;

        if (currentHealth > 0)
        {
            //play sound
            //animate
            //UI remove heart -- when you have the time
            UIController
                .UpdateSliderAmount(UIController.UIImageComponents.healthBar,
                maxHealth,
                currentHealth);
        }
        else
        {
            //game over! (controlled by state controller)
        }
    }

    public static void AddHealth(int healthAdded)
    {
        if (currentHealth < maxHealth) currentHealth += healthAdded;
        UIController
            .UpdateSliderAmount(UIController.UIImageComponents.healthBar,
            maxHealth,
            currentHealth);
        // UI add heart -- when you have time
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

    public static void AddShield()
    {
        hasShield = true;
        //updateUI
        //animate
        //sound
    }

    public static bool NotMaxHealth() => currentHealth < maxHealth;
}
