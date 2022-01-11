using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStateController
{
    public static bool gamePaused;

    public static States state;

    public enum States
    {
        Intro,
        Tutorial,
        Play,
        Arena,
        Shopping,
        Paused,
        MainMenu
    }

    public static void PauseGame()
    {
        if (!gamePaused)
        {
            Time.timeScale = 0;
            gamePaused = true;
            //show menu
        }
        else
        {
            Time.timeScale = 1;
            gamePaused = false;
            if (GameStateController.state == GameStateController.States.Tutorial
            )
                TutorialManager
                    .CheckTutorialStatic(Tutorial.TutorialValidation.Menu);
            //hide menu
        }
    }

    public static void Quit()
    {
        //save prefs here
        Application.Quit();
    }
}
