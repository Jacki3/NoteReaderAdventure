using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public GameObject mainButtons;

    public GameObject optionButtons;

    public Canvas gameCanvas;

    public Animator menuAnimator;

    private bool optionsVisible;

    private void Awake()
    {
        GameStateController.PauseGame();
        gameCanvas.enabled = false;
    }

    public void StartGame()
    {
        menuAnimator.SetTrigger("StartGame");
        gameObject.SetActive(false);
        GameStateController.PauseGame();
        gameCanvas.enabled = true;
    }

    public void ShowOptions()
    {
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
        GameStateController.Quit();
    }
}
