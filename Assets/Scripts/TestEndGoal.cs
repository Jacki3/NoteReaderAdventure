using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEndGoal : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (
                LevelController.i.levelLoader.boardController.notations.Count <=
                1
            )
                LevelController.i.LevelComplete();
            else
            {
                Tooltip
                    .SetToolTip_Static("Notations Left to Complete!",
                    Vector3.zero,
                    CoreGameElements.i.mainCanvas.transform);
            }
        }
    }
}
