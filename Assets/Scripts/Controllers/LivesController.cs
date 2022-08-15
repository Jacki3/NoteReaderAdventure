using UnityEngine;

public static class LivesController
{
    private static int currentLives;

    public static void SetLives()
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
        HealthController.ResetHealth();
        HealthController.SaveHealth();
        EndScreens.ShowLifeOverStatic();
        CoreGameElements.i.gameSave.livesLost++;
        UpdateLivesUI();
        return false;
    }

    public static void UpdateLivesUI()
    {
        int livesLost = CoreGameElements.i.gameSave.livesLost;
        UIController
            .UpdateTextUI(UIController.UITextComponents.livesText,
            livesLost.ToString());
        UIController
            .UpdateTextUI(UIController.UITextComponents.livesLostMenu,
            livesLost.ToString());
    }

    private static void UpdateUI()
    {
    }
}
