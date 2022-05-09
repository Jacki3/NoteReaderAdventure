using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarController : NotationItem
{
    public Gate gate;

    public SpriteRenderer glow;

    public SpriteRenderer gateGlow;

    public int totalToActivate;

    private int currentlyActivated;

    protected override void AllNotationsComplete()
    {
        base.AllNotationsComplete();
        HighlightGlow();
        Activate();
    }

    public void HighlightGlow()
    {
        //lerp this
        glow.enabled = true;
        gateGlow.color = Color.white;
    }

    public void Activate()
    {
        currentlyActivated++;

        if (AllActivated())
        {
            gate.OpenGate();
            SoundController.PlaySound(SoundController.Sound.GateOpen);
        }
    }

    private bool AllActivated() => currentlyActivated >= totalToActivate;
}
