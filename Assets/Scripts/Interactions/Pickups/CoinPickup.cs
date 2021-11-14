using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : Item
{
    protected override void PickUp()
    {
        //animate
        base.PickUp();
        SoundController.PlaySound(SoundController.Sound.CoinPickup);
        Destroy (gameObject);
        //destroy on animate - make an animation rather than code it!
        //add to currency (updating UI)
    }
}
