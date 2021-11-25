using UnityEngine;

[System.Serializable]
public class CoreItems
{
    public enum ItemType
    {
        shield,
        protectiveShield,
        smallHealthRefill,
        healthRefill,
        largeHealthRefill,
        coolSunGlasses,
        nerdGlasses,
        life
    }

    public ItemType item;

    public string itemName;

    public int cost;

    public Sprite itemSprite;

    public static int GetCost(CoreItems itemType)
    {
        return itemType.cost;
    }
}
