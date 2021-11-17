using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStateController
{
    public static bool gamePaused;

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
            //hide menu
        }
    }
}
