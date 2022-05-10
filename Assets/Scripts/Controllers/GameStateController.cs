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
            AudioListener.pause = true;
            Time.timeScale = 0; //is this correct or should we also be pausing specific things? check docs if time provides
            gamePaused = true;
        }
        else
        {
            AudioListener.pause = false;
            Time.timeScale = 1;
            gamePaused = false;
            if (GameStateController.state == GameStateController.States.Tutorial
            )
                TutorialManager
                    .CheckTutorialStatic(Tutorial.TutorialValidation.Menu);
        }
    }

    public static void Quit()
    {
        //save prefs here
        Application.Quit();
    }
}
