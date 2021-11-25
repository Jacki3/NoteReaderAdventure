using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public enum ItemType
    {
        Coin,
        SmallHealthPotion,
        HealthPotion,
        LargeHealthPotion,
        Spirit
        //etc.
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
            newItem.transform.position = spawnPos + Random.insideUnitCircle * 1;

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
}
