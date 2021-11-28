using UnityEngine;

public static class LivesController
{
    private static int currentLives;

    [RuntimeInitializeOnLoadMethod]
    private static void SetLives()
    {
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

    private static bool GameOver()
    {
        if (currentLives <= 0)
        {
            Debug.Log("GAME OVER!"); //called in state controller
            return true;
        }
        else
        {
            Debug.Log("LIFE LOST! START AT CHECKPOINT!"); //call load level state controller
            HealthController.ResetHealth();
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
