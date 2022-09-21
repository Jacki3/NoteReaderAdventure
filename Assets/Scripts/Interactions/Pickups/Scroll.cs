using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : Collectible
{
    protected override void PickUp()
    {
        base.PickUp();
        LevelController.i.levelLoader.CollectScrollForLevel();
    }
}
