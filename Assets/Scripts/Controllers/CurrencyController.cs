using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrencyController
{
    private static int totalCoinsCollected = 0;

    public static void AddRemoveCoins(int coins, bool addOrRemove)
    {
        totalCoinsCollected =
            addOrRemove
                ? totalCoinsCollected += coins
                : totalCoinsCollected -= coins;

        UIController
            .UpdateTextUI(UIController.UITextComponents.coinText,
            totalCoinsCollected.ToString());
    }

    public static int GetTotalCoins()
    {
        return totalCoinsCollected;
    }
}
