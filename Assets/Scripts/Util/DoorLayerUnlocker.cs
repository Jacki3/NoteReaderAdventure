using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorLayerUnlocker : MonoBehaviour
{
    TilemapCollider2D tilemapCollider2D;

    private void Start()
    {
        tilemapCollider2D = GetComponent<TilemapCollider2D>();
    }

    public void LayerCheck(SpriteRenderer player)
    {
        if (player.sortingLayerName == "Layer 1")
        {
            tilemapCollider2D.enabled = false;
            Invoke("DelayColliderOn", 1f);
        }
        else
        {
            tilemapCollider2D.enabled = true;
        }
    }

    private void DelayColliderOn()
    {
        tilemapCollider2D.enabled = true;
    }
}
