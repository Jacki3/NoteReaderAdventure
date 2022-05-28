using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreens : MonoBehaviour
{
    public GameObject gameOverScreen;

    public GameObject lifeLostScreen;

    public Canvas mainCanvas;

    public Animator backgroundAnimator;

    private Canvas canvas;

    private static EndScreens instance;

    private void Awake()
    {
        canvas = transform.GetChild(0).GetComponent<Canvas>();
        instance = this;
    }

    private void Update()
    {
        if (gameOverScreen.activeSelf)
        {
            if (
                PlayerController
                    .inputActions
                    .Player
                    .Escape
                    .WasPressedThisFrame()
            ) Application.Quit();
        }
    }

    public static void ShowGameOverStatic() => instance.ShowGameOver();

    public static void ShowLifeOverStatic() => instance.ShowLifeOver();

    public void ShowGameOver()
    {
        mainCanvas.enabled = false;
        GameStateController.PauseGame(true);
        canvas.enabled = true;
        gameOverScreen.SetActive(true);
    }

    public void ShowLifeOver()
    {
        mainCanvas.enabled = false;
        GameStateController.PauseGame(true);
        canvas.enabled = true;
        lifeLostScreen.SetActive(true);
    }

    public void ResumeGame()
    {
        backgroundAnimator.SetTrigger("Resume");
        mainCanvas.enabled = true;
        GameStateController.PauseGame(true);
        lifeLostScreen.SetActive(false);
    }
}
