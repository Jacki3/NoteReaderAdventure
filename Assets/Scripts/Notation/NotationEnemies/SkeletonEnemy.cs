using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonEnemy : RhythmEnemy
{
    protected override void OnCantMove<T>(T Component, int x, int y)
    {
        RigidPlayerController hitPlayer = Component as RigidPlayerController;
        if (Component == hitPlayer) hitPlayer.LoseHealth(playerDmg);
    }

    protected override void CorrectNote()
    {
        animator.SetTrigger("Hurt");
    }
}
