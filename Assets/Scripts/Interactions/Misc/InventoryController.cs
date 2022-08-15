using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public IventoryItem[] iventoryItems;

    private static InventoryController i;

    private void Awake()
    {
        i = this;
    }

    public static bool CheckItem(CoreItems.ItemType item)
    {
        if (i.CheckItemUnlocked(item))
            return true;
        else
            return false;
    }

    public static bool CheckBoughtItemStatic(CoreItems.ItemType item)
    {
        if (i.CheckBoughtItem(item))
            return true;
        else
            return false;
    }

    private bool CheckItemUnlocked(CoreItems.ItemType item)
    {
        foreach (IventoryItem invetoryItem in iventoryItems)
        {
            if (item == invetoryItem.item && invetoryItem.isUnlocked)
                return true;
        }
        return false;
    }

    public bool CheckBoughtItem(CoreItems.ItemType itemBought)
    {
        foreach (IventoryItem item in iventoryItems)
        {
            if (itemBought == item.item && !item.isUnlocked)
            {
                item.isUnlocked = true;
                item.itemSprite.enabled = true;
                SaveItems();
                return true;
            }
            else
            {
                print("item already purchased");
                return false;
            }
        }
        return false;
    }

    public void SaveItems()
    {
        var save = CoreGameElements.i.gameSave;
        save.iventoryItems = iventoryItems.ToArray();

        for (int i = 0; i < iventoryItems.Length; i++)
        {
            save.iventoryItems[i].isUnlocked = iventoryItems[i].isUnlocked;
        }
    }

    public static void LoadItemsStatic()
    {
        i.LoadItems();
    }

    public void LoadItems()
    {
        IventoryItem[] savedItems = CoreGameElements.i.gameSave.iventoryItems;
        if (savedItems.Length > 0)
            for (int i = 0; i < iventoryItems.Length; i++)
            {
                iventoryItems[i].isUnlocked = savedItems[i].isUnlocked;
            }
    }
}
