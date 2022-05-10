﻿using System.Collections;
using UnityEngine;

public class RigidPlayerController : MovingObject
{
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    private void Update()
    {
        inverseMoveTime = 1f / moveTime;

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        if (PlayerController.inputActions.Player.Up.WasPressedThisFrame())
            AttemptMove(0, 1);
        else if (PlayerController.inputActions.Player.Down.WasPressedThisFrame()
        )
            AttemptMove(0, -1);
        else if (PlayerController.inputActions.Player.Left.WasPressedThisFrame()
        )
            AttemptMove(-1, 0);
        else if (
            PlayerController.inputActions.Player.Right.WasPressedThisFrame()
        ) AttemptMove(1, 0);
    }

    protected override void AttemptMove(int xDir, int yDir)
    {
        base.AttemptMove(xDir, yDir);

        RaycastHit2D hit;

        if (Move(xDir, yDir, out hit))
        {
            if (AudioController.canPlay)
            {
                ScoreController.AddStreak_Static();
                //flash rhythm anim
                //update score multiplier logic (add glow to rhythm indicator)
            }
            else
            {
                ScoreController.ResetStreak_Static(true);
            }
            //play sound etc.
        }
    }

    protected override void OnCantMove<T>(T Component)
    {
        throw new System.NotImplementedException();
    }
}
