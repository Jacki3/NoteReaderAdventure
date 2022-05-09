using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : NotationItem
{
    public SpriteRenderer glow;

    public SpriteRenderer gateGlow;

    public PillarController pillarController;

    protected override void AllNotationsComplete()
    {
        base.AllNotationsComplete();
        Activate();
    }

    private void Activate()
    {
        //lerp this
        glow.enabled = true;
        gateGlow.color = Color.white;
        pillarController.Activate();
    }
}
