﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEndGoal : MonoBehaviour
{
    private BoardController boardController;

    void Start()
    {
        boardController = LevelController.i.levelLoader.boardController;
    }

    private void Update()
    {
        print(GameStateController.state);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (!LevelController.i.levelLoader.usingCustomLvl)
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
            else if (
                GameStateController.state != GameStateController.States.Tutorial
            )
            {
                Seed.LevelObjs[] customLevels =
                    LevelController.i.levelLoader.customLevel;
                int customIndex = LevelController.i.levelLoader.customLevelNum;
                if (customLevels[customIndex].AllNotationsComplete())
                {
                    LevelController.i.LevelComplete();
                }
                {
                    int notationsLeft =
                        customLevels[customIndex].levelNotations.Count;
                    string msg = notationsLeft + " Notations Left To Complete!";
                    Tooltip
                        .SetToolTip_Static(msg,
                        Vector3.zero,
                        CoreGameElements.i.mainCanvas.transform);
                }
            }
            else
            {
                if (TutorialManager.TutorialComplete())
                {
                    TutorialManager.StartGameStatic();
                    CoreGameElements.i.useTutorial = false;
                    LevelController.i.LevelComplete();
                }
                else
                {
                    string msg = "Please finish the tutorial!";
                    Tooltip
                        .SetToolTip_Static(msg,
                        Vector3.zero,
                        CoreGameElements.i.mainCanvas.transform);
                }
            }
        }
    }
}
