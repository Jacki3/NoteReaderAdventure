using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrencyController
{
    public static int totalCoinsCollected = 0;

    private static int totalCollectiblesCollected = 0;

    public static void AddRemoveCoins(int coins, bool add)
    {
        if (add)
        {
            totalCoinsCollected += coins;
            UIController
                .UpdateTextUI(UIController.UITextComponents.coinText,
                totalCoinsCollected.ToString());
            UIController
                .UpdateTextUI(UIController.UITextComponents.shopCoinText,
                totalCoinsCollected.ToString());
            // ScoreDisplayUpdater
            //     .StartRoutine(totalCoinsCollected,
            //     UIController.UITextComponents.coinText);
            // ScoreDisplayUpdater
            //     .StartRoutine(totalCoinsCollected,
            //     UIController.UITextComponents.shopCoinText);
        }
        else
        {
            totalCoinsCollected -= coins;
            if (totalCoinsCollected <= 0) totalCoinsCollected = 0;
            UIController
                .UpdateTextUI(UIController.UITextComponents.coinText,
                totalCoinsCollected.ToString());
            UIController
                .UpdateTextUI(UIController.UITextComponents.shopCoinText,
                totalCoinsCollected.ToString());
            // ScoreDisplayUpdater
            //     .StartRoutineDown(totalCoinsCollected,
            //     UIController.UITextComponents.coinText);
            // ScoreDisplayUpdater
            //     .StartRoutineDown(totalCoinsCollected,
            //     UIController.UITextComponents.shopCoinText);
        }
    }

    public static int GetTotalCoins()
    {
        return totalCoinsCollected;
    }

    public static void AddCollectible()
    {
        totalCollectiblesCollected++;
        UIController
            .UpdateTextUI(UIController.UITextComponents.collectibleText,
            totalCollectiblesCollected +
            "/" +
            CoreGameElements.i.totalCollectiblesThisLevel);
    }
}
