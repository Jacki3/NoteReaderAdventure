using UnityEngine;

public static class LivesController
{
    private static int currentLives;

    [RuntimeInitializeOnLoadMethod]
    private static void SetLives()
    {
        if (CoreGameElements.i != null)
            currentLives = CoreGameElements.i.totalLives;
        UIController
            .UpdateTextUI(UIController.UITextComponents.livesText,
            currentLives.ToString());
    }

    public static void AddLife()
    {
        currentLives++;
        UpdateUI();
    }

    public static void RemoveLife()
    {
        currentLives--;
        UpdateUI();
        GameOver();
    }

    public static bool GameOver()
    {
        if (currentLives <= 0)
        {
            EndScreens.ShowGameOverStatic();
            return true;
        }
        else
        {
            HealthController.ResetHealth();
            EndScreens.ShowLifeOverStatic();
            return false;
        }
    }

    private static void UpdateUI()
    {
        UIController
            .UpdateTextUI(UIController.UITextComponents.livesText,
            currentLives.ToString());
    }
}
