using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarGate : Gate
{
    public AreaTrigger areaTrigger;

    public override void TryGate()
    {
        if (!gateOpen)
        {
            SoundController.PlaySound(SoundController.Sound.DoorLocked);
            Tooltip
                .SetToolTip_Static("Gate Opens Elsewhere...",
                Vector3.zero,
                mainCanvas);
        }
        else
        {
            areaTrigger.ShowArea(true);
        }
    }

    public override void OpenGate()
    {
        gateOpen = true;
        gateRenderer.sprite = openGate;
    }
}
