using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IventoryItem
{
    public CoreItems.ItemType item;

    public string itemName;

    public UnityEngine.UI.Image itemSprite;

    public bool isUnlocked;
}
