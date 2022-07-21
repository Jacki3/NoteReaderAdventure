using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullObstacleEnemy : RhythmEnemy
{
    protected override void OnCantMove<T>(T Component)
    {
        RigidPlayerController hitPlayer = Component as RigidPlayerController;
        if (Component == hitPlayer) hitPlayer.LoseHealth(playerDmg);
    }

    protected override void CorrectNote()
    {
        animator.SetTrigger("Hurt");
    }
}
