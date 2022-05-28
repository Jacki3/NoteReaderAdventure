using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : Item
{
    protected override void PickUp()
    {
        base.PickUp();
        CurrencyController.AddCollectible();
        Destroy (gameObject);
    }
}
