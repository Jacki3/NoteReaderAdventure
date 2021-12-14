using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : NotationItem, INotation
{
    public Sprite openGate;

    private SpriteRenderer gateRenderer;

    private BoxCollider2D gateCollider;

    private void Awake()
    {
        gateRenderer = GetComponent<SpriteRenderer>();
        gateCollider = GetComponent<BoxCollider2D>();
    }

    public override void NotationComplete()
    {
        base.NotationComplete();
        Invoke("OpenGate", 1);
    }

    public void OpenGate()
    {
        gateRenderer.sprite = openGate;
        gateCollider.enabled = false;
    }
}
