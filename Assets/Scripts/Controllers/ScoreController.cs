﻿using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public float stackedTime = 5;

    public float stackTimeToAdd = .25f;

    public int score;

    private int streak;

    private int multiplier;

    private int previousScore;

    private bool canStackScore;

    private float spawnTime;

    private float lifeTime = 0;

    private static ScoreController instance;

    int previousScoreToAdd;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        float progress = (Time.time - spawnTime) / lifeTime;

        if (progress < 1)
        {
            canStackScore = true;
        }
        else
        {
            canStackScore = false;
            previousScoreToAdd = 0;
            previousScore = 0;
        }

        float timer = canStackScore ? stackedTime - progress * stackedTime : 0;
        UIController
            .UpdateSliderAmount(UIController.UIImageComponents.multiplierBar,
            stackedTime,
            timer);
    }

    public void AddScore(int scoreToAdd)
    {
        streak++;

        int totalScoreToAdd = previousScoreToAdd + scoreToAdd + previousScore;
        previousScoreToAdd = totalScoreToAdd;

        score += totalScoreToAdd;
        ScoreDisplayUpdater
            .StartRoutine(score, UIController.UITextComponents.scoreText);

        ScoreGain.SetScoreGain_Static("+" + totalScoreToAdd);

        if (!canStackScore)
        {
            spawnTime = Time.time;
            lifeTime = stackedTime; //this could be an upgrade
        }
        else
        {
            lifeTime += .5f; //this could also be upgrade

            previousScore = totalScoreToAdd / 4; //look at this math logic
        }
    }

    public void ResetStreak() => streak = 0;

    public static void AddScore_Static(int scoreToAdd)
    {
        instance.AddScore (scoreToAdd);
    }
}