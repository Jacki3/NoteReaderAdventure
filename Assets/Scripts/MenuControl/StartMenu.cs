using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public GameObject mainButtons;

    public GameObject optionButtons;

    public Canvas gameCanvas;

    public Canvas pauseMenu;

    public Animator menuAnimator;

    private bool optionsVisible;

    private void Awake()
    {
        // GameStateController.PauseGame();
        gameCanvas.enabled = false;
    }

    public void StartGame()
    {
        // GameStateController.PauseGame(); //perhaps change this for state behaviour so game is always running but rather than pausing it switches to state of play within main menu + add a state for main menu and pause

        menuAnimator.SetTrigger("Start");
        gameCanvas.enabled = true;
    }

    public void ShowOptions()
    {
        SoundController.PlaySound(SoundController.Sound.ButtonClick);
        if (optionsVisible)
        {
            optionButtons.SetActive(false);
            mainButtons.SetActive(true);
            optionsVisible = false;
        }
        else
        {
            optionButtons.SetActive(true);
            mainButtons.SetActive(false);
            optionsVisible = true;
        }
    }

    public void Quit()
    {
        SoundController.PlaySound(SoundController.Sound.ButtonClick);
        GameStateController.Quit();
    }
}
