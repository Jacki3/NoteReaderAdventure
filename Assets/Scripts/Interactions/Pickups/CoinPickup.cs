using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : Item
{
    public int valueOfCoin = 1;

    protected override void PickUp()
    {
        base.PickUp();
        CurrencyController.AddRemoveCoins(valueOfCoin, true);
        Destroy (gameObject);
    }
}
