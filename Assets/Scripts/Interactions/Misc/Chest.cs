using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : NotationItem
{
    public Sprite openChest;

    public ItemSpawner.ItemType itemToSpawn;

    public int totalItemsToSpawn;

    private SpriteRenderer chestRenderer;

    private void Awake()
    {
        chestRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void AllNotationsComplete()
    {
        base.AllNotationsComplete();
        OpenChest();
    }

    private void OpenChest()
    {
        Vector2 spawnPos =
            new Vector2(transform.position.x, transform.position.y - 1f);

        // new Vector2(transform.position.x + 2, transform.position.y + 2f);
        chestRenderer.sprite = openChest;
        ItemSpawner.SpawnItem (
            itemToSpawn,
            spawnPos,
            totalItemsToSpawn,
            chestRenderer
        );
    }
}
