using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrencyController
{
    private static int totalCoinsCollected = 0;

    private static int totalCollectiblesCollected = 0;

    public static void AddRemoveCoins(int coins, bool add)
    {
        // totalCoinsCollected =
        //     addOrRemove
        //         ? totalCoinsCollected += coins
        //         : totalCoinsCollected -= coins;
        if (add)
        {
            totalCoinsCollected += coins;
            ScoreDisplayUpdater
                .StartRoutine(totalCoinsCollected,
                UIController.UITextComponents.coinText);
        }
        else
        {
            totalCoinsCollected -= coins;
            if (totalCoinsCollected <= 0) totalCoinsCollected = 0;
            ScoreDisplayUpdater
                .StartRoutineDown(totalCoinsCollected,
                UIController.UITextComponents.coinText);
        }

        CoreGameElements.i.gameSave.playerCoins = totalCoinsCollected;
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
