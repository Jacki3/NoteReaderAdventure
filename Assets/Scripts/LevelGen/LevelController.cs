using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public Transform player;

    public Transform mainCam;

    public PauseMenu pauseMenu;

    public Vector3 customSpawnPoint;

    [HideInInspector]
    public Seed levelLoader;

    private static LevelController _i;

    private Vector3 playerDefaultPos;

    public static LevelController i
    {
        get
        {
            return _i;
        }
    }

    private void Awake()
    {
        if (_i != null && _i != this)
            Destroy(this.gameObject);
        else
            _i = this;

        levelLoader = GetComponent<Seed>();
    }

    void Start()
    {
        playerDefaultPos = player.position;
    }

    public void LevelComplete()
    {
        CoreGameElements.i.gameSave.playerCoins =
            CurrencyController.GetTotalCoins();
        ExperienceController.SaveXP();
        HealthController.SaveHealth();
        MissionHolder.i.SaveAllMissions();
        PlayerSkills.SaveAllSkills();
        UIController.SaveUIHearts();

        int nextLevel = CoreGameElements.i.gameSave.levelAt;

        if (nextLevel == 0) nextLevel = 1;
        int currentLevel = levelLoader.currentLevel;

        int score = ScoreController.GetScoreStatic();
        int highScore =
            CoreGameElements.i.gameSave.boards[currentLevel - 1].score;
        if (score > highScore)
            CoreGameElements.i.gameSave.boards[currentLevel - 1].score = score;
        ScoreController.ResetScoreStatic();

        if (currentLevel < CoreGameElements.i.totalLevels)
        {
            if (currentLevel + 1 > nextLevel)
            {
                nextLevel++;
                CoreGameElements.i.latetstLevel = nextLevel;
                CoreGameElements.i.gameSave.levelAt = nextLevel;
            }

            player.GetComponent<BoxCollider2D>().enabled = false;
            FXController
                .SetAnimatorTrigger_Static(FXController.Animations.LevelFader,
                "Fade");
            DelayResetPlayer();

            //load a puzzle every odd level, load a shop every fifth level or just load the next level
            if (
                currentLevel != 1 &&
                currentLevel % 2 == 1 &&
                currentLevel % 5 != 0
            )
                pauseMenu.ReturnToMain(true, false);
            else if (currentLevel % 5 == 0)
                pauseMenu.ReturnToMain(false, true);
            else
                levelLoader.LoadLevel(currentLevel + 1);
        }
        else
        {
            EndScreens.ShowGameCompleteStatic();
        }
    }

    public void ResetPlayerPos()
    {
        player.GetComponent<BoxCollider2D>().enabled = false;
        Invoke("ResetPlayer", .5f);
    }

    public void DelayResetPlayer()
    {
        Invoke("ResetPlayer", .2f);
    }

    private void ResetPlayer()
    {
        player.position = playerDefaultPos;
        mainCam.position = Vector3.zero;
        player.GetComponent<BoxCollider2D>().enabled = true;
    }

    private void ResetPlayerCustom()
    {
        player.position = customSpawnPoint;
        mainCam.position = Vector3.zero; //might not be right
        player.GetComponent<BoxCollider2D>().enabled = true;
    }
}
