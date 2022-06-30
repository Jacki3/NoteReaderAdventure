using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public float stackedTime = 5;

    public float stackTimeToAdd = .25f;

    public int score;

    public int streakToMultiply = 5;

    public float expoStreakToMultiply;

    public int maxMultiplier = 8;

    public float glowAmount;

    private int streak;

    private int multiplier;

    private int previousScore;

    private bool canStackScore;

    private float spawnTime;

    private float lifeTime = 0;

    private static ScoreController instance;

    private int scoreMultiplier = 1;

    private int defaultStreakToMultiply;

    private int previousScoreToAdd;

    private ShaderExpose beatGlowShader;

    private float defaultGlowAmount;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        scoreMultiplier = 1;
        defaultStreakToMultiply = streakToMultiply;
        defaultGlowAmount = glowAmount;
        var beatGlowImg =
            CoreUIElements
                .i
                .GetImageComponent(UIController.UIImageComponents.outerBeat);
        if (beatGlowImg != null)
        {
            beatGlowShader = beatGlowImg.GetComponent<ShaderExpose>();
        }
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

    private void AddScore(int scoreToAdd)
    {
        int totalScoreToAdd =
            previousScoreToAdd + (scoreToAdd * scoreMultiplier) + previousScore; //this seems crazy
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
        CoreGameElements.i.gameSave.overallScore += score;
    }

    public static void AddScore_Static(int scoreToAdd)
    {
        instance.AddScore (scoreToAdd);
    }

    public void AddRhythmScore(int scoreToAdd)
    {
        score += scoreToAdd;
        ScoreDisplayUpdater
            .StartRoutine(score, UIController.UITextComponents.scoreText);

        ScoreGainRhythm.SetScoreGain_Static("+rhythm bonus!");
    }

    public static void AddRhythmScore_Static(int scoreToAdd)
    {
        instance.AddRhythmScore (scoreToAdd);
    }

    private void AddStreak()
    {
        streak++;
        if (scoreMultiplier < maxMultiplier)
        {
            float incrementGlow = glowAmount / streakToMultiply;
            if (beatGlowShader != null)
                beatGlowShader._myCustomFloat += incrementGlow;
            AddMultiplier();
        }
        UIController
            .UpdateTextUI(UIController.UITextComponents.streakText,
            streak.ToString());
        FXController
            .SetAnimatorTrigger_Static(FXController.Animations.BeatFlash,
            "Flash");
    }

    private void AddMultiplier()
    {
        if (streak >= streakToMultiply)
        {
            glowAmount += glowAmount;
            streakToMultiply =
                (int) Mathf.Pow(streakToMultiply, expoStreakToMultiply);
            scoreMultiplier += scoreMultiplier;
            UIController
                .UpdateTextUI(UIController.UITextComponents.multiplyText,
                "x " + scoreMultiplier);
        }
    }

    public static void AddStreak_Static()
    {
        instance.AddStreak();
    }

    private void ResetStreak(bool rhythmMissed)
    {
        streak = 0;
        scoreMultiplier = 1;
        streakToMultiply = defaultStreakToMultiply;
        glowAmount = defaultGlowAmount;
        if (beatGlowShader != null) beatGlowShader._myCustomFloat = 0;

        UIController
            .UpdateTextUI(UIController.UITextComponents.multiplyText,
            "x " + scoreMultiplier);
        UIController
            .UpdateTextUI(UIController.UITextComponents.streakText,
            streak.ToString());
        FXController
            .SetAnimatorTrigger_Static(FXController.Animations.BeatFlash,
            "Reset");

        if (rhythmMissed)
        {
            ScoreGainRhythm.SetScoreGain_Static("missed beat!");
        }
    }

    public static void ResetStreak_Static(bool rhythmMissed)
    {
        instance.ResetStreak (rhythmMissed);
    }

    public static int GetMaxMultiplierStatic()
    {
        return instance.GetMaxMultiplier();
    }

    public int GetMaxMultiplier()
    {
        return maxMultiplier;
    }

    public static int GetScoreStatic()
    {
        return instance.GetScore();
    }

    public int GetScore()
    {
        return score;
    }

    public static void ResetScoreStatic()
    {
        instance.ResetScore();
    }

    public void ResetScore()
    {
        score = 0;
        ScoreDisplayUpdater
            .StartRoutineDown(score, UIController.UITextComponents.scoreText);
    }
}
