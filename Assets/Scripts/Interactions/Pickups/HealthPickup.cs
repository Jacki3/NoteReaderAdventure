using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Item
{
    public int healthToAdd = 1;

    protected override void PickUp()
    {
        base.PickUp();
        HealthController.AddHealth (healthToAdd);
        Destroy (gameObject);
    }
}
