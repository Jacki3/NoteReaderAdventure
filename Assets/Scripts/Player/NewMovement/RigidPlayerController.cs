using System.Collections;
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
        RaycastHit2D hit;
        if (!GameStateController.gamePaused)
        {
            if (Move(xDir, yDir, out hit))
            {
                if (AudioController.canPlay)
                {
                    ScoreController.AddStreak_Static();
                }
                else
                {
                    ScoreController.ResetStreak_Static(true);
                }
                //play sound etc.
            }
        }
    }

    protected override void OnCantMove<T>(T Component)
    {
        throw new System.NotImplementedException();
    }
}
