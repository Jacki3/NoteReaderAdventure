﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : NotationItem, INotation
{
    public Sprite openChest;

    public ItemSpawner.ItemType itemToSpawn;

    public int totalItemsToSpawn;

    private SpriteRenderer chestRenderer;

    private void Awake()
    {
        chestRenderer = GetComponent<SpriteRenderer>();
    }

    public override void NotationComplete()
    {
        base.NotationComplete();
        OpenChest();
    }

    private void OpenChest()
    {
        Vector2 spawnPos =
            new Vector2(transform.position.x + 1, transform.position.y + .5f);
        chestRenderer.sprite = openChest;
        ItemSpawner.SpawnItem (
            itemToSpawn,
            spawnPos,
            totalItemsToSpawn,
            chestRenderer
        );
    }
}
