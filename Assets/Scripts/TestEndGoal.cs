using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEndGoal : MonoBehaviour
{
    private BoardController boardController;

    void Start()
    {
        boardController = LevelController.i.levelLoader.boardController;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (boardController.notations.Count <= 0)
                LevelController.i.LevelComplete();
            else
            {
                int notationsLeft = boardController.notations.Count;
                string msg = notationsLeft + " Notations Left To Complete!";
                Tooltip
                    .SetToolTip_Static(msg,
                    Vector3.zero,
                    CoreGameElements.i.mainCanvas.transform);
            }
        }
    }
}
