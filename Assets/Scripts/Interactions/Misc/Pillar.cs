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
        if (gateGlow != null) gateGlow.color = Color.white;
        if (pillarController != null) pillarController.Activate();
    }
}
