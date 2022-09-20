﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyChest : MonoBehaviour
{
    public Sprite openChest;

    public ItemSpawner.ItemType itemToSpawn;

    public Mission.Object missionObject;

    [SerializeField]
    private Key.KeyType keyType;

    public int totalItemsToSpawn;

    public Transform mainCanvas;

    public UIController.UIImageComponents keyImage;

    private SpriteRenderer chestRenderer;

    private void Awake()
    {
        chestRenderer = GetComponent<SpriteRenderer>();
    }

    public void TryOpenChest()
    {
        if (KeyHolder.ContainsKey(keyType))
        {
            KeyHolder.RemoveKey (keyType);
            MissionHolder.i.CheckValidMission (missionObject);
            UIController.UpdateImageSprite(keyImage, null, false);
            Tooltip
                .SetToolTip_Static("Used " + keyType + " Key!",
                Vector3.zero,
                mainCanvas);
        }
        else
        {
            SoundController.PlaySound(SoundController.Sound.DoorLocked);
            Tooltip
                .SetToolTip_Static("Requires " + keyType + " Key!",
                Vector3.zero,
                mainCanvas);
        }
    }

    private void OpenChest()
    {
        Vector2 spawnPos =
            new Vector2(transform.position.x + 2, transform.position.y + 2f); //change this
        chestRenderer.sprite = openChest;
        ItemSpawner.SpawnItem (
            itemToSpawn,
            spawnPos,
            totalItemsToSpawn,
            chestRenderer
        );
    }
}
