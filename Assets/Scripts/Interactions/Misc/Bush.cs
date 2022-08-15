using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    public GameObject flowers;

    public void WaterBush()
    {
        CoreItems.ItemType water = CoreItems.ItemType.water;
        if (InventoryController.CheckItem(water))
        {
            if (flowers != null) flowers.SetActive(true);
            //drop items
            //make watering sound
        }
        else
            print(water + " locked");
    }
}
