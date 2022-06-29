using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class ItemSpawner : MonoBehaviour
{
    public enum ItemType
    {
        BronzeCoin,
        SilverCoin,
        GoldCoin,
        UltraCoin,
        SmallHealthPotion,
        HealthPotion,
        LargeHealthPotion,
        Spirit,
        GoldenKey,
        SilverKey,
        IronKey
    }

    public static List<Item> allItems = new List<Item>();

    public static void SpawnItem(
        ItemType item,
        Vector2 spawnPos,
        int maxItems,
        SpriteRenderer spriteRenderer
    )
    {
        int randMax = Random.Range(1, maxItems);
        for (int i = 0; i < randMax; i++)
        {
            Item newItem = Instantiate(GetItem(item));

            // var roundedX =
            //     Math.Round(newPos.x, 0, MidpointRounding.AwayFromZero);
            // var roundedY =
            //     Math.Round(newPos.y, 0, MidpointRounding.AwayFromZero);
            // newPos = new Vector2((float) roundedX, (float) roundedY);
            newItem.transform.position = spawnPos;

            //apply same logic to player movement casting to ensure object is not in same tile as prop etc.
            SpriteRenderer _spriteRenderer =
                newItem.GetComponent<SpriteRenderer>();
            _spriteRenderer.sortingLayerName = spriteRenderer.sortingLayerName;
            _spriteRenderer.sortingLayerID = spriteRenderer.sortingLayerID;

            allItems.Add (newItem);
        }
    }

    private static Item GetItem(ItemType itemType)
    {
        foreach (CoreItemElements.ItemType item in CoreItemElements.i.itemTypes)
        {
            if (item.item == itemType) return item.itemType;
        }
        Debug.LogError("ItemType" + itemType + "missing!");
        return null;
    }

    public static void RemoveItem(Item item)
    {
        allItems.Remove (item);
    }

    public static void ClearItems()
    {
        foreach (Item item in allItems) Destroy(item.gameObject);
        allItems.Clear();
    }
}
