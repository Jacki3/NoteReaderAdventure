using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : Item
{
    protected override void PickUp()
    {
        base.PickUp();
        CurrencyController.AddRemoveCoins(1, true);
        MissionHolder.i.CheckValidMission(Mission.Object.Coins);
        Destroy (gameObject);
        //destroy on animate - make an animation rather than code it!
        //add to currency (updating UI)
    }
}
