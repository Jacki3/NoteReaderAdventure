﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EndScreens : MonoBehaviour
{
    public GameObject firstButton;

    public GameObject gameOverScreen;

    public GameObject lifeLostScreen;

    public Canvas mainCanvas;

    public Animator backgroundAnimator;

    private Canvas canvas;

    private static EndScreens instance;

    private void Awake()
    {
        instance = this;
        canvas = transform.GetChild(0).GetComponent<Canvas>();
    }

    public static void ShowGameOverStatic() => instance.ShowGameOver();

    public static void ShowLifeOverStatic() => instance.ShowLifeOver();

    public static void HideScreensStatic() => instance.HideScreens();

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
        EventSystem.current.SetSelectedGameObject (firstButton);
    }

    public void ResumeGame()
    {
        backgroundAnimator.SetTrigger("Resume");
        mainCanvas.enabled = true;
        GameStateController.PauseGame(true);
        lifeLostScreen.SetActive(false);
        LevelController.i.ResetPlayerPos();
        LevelController.i.levelLoader.LoadLevel(-2);
    }

    private void HideScreens()
    {
        canvas.enabled = false;
        lifeLostScreen.SetActive(false);
    }
}
