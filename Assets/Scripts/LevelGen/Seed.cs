using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public string gameSeed = "Default";

    public int currentSeed;

    public int level = 1;

    public int currentLevel;

    public BoardController boardController;

    public string defaultGameSeed;

    public void SetLevels()
    {
        Debug.Log("First Run!");
        int totalLevels = CoreGameElements.i.totalLevels;
        GenerateAllLevels (totalLevels);
    }

    public void GenerateAllLevels(int totalLevels)
    {
        for (int i = 0; i < totalLevels; i++)
        {
            SaveFile.Board newBoard = new SaveFile.Board();

            gameSeed = "LvlBuilder1_";
            defaultGameSeed = gameSeed;
            gameSeed += level;
            currentSeed = gameSeed.GetHashCode();
            Random.InitState (currentSeed);
            boardController.SetupScene (level);

            CoreGameElements.i.gameSave.defaultGameSeed = defaultGameSeed;

            newBoard.level = level;
            newBoard.rowsMins = boardController.rowsMin;
            newBoard.rowsMax = boardController.rowsMax;
            newBoard.columsMin = boardController.columsMin;
            newBoard.columnsMax = boardController.columnsMax;
            newBoard.propsMin = boardController.propCount.minimum;
            newBoard.propsMax = boardController.propCount.maximum;
            newBoard.maxScore =
                boardController.boardMaxScore *
                ScoreController.GetMaxMultiplierStatic();

            CoreGameElements.i.gameSave.boards.Add (newBoard);

            level++;

            //if final level to gen, delete it so it does not play in background
            if (i == totalLevels - 1) boardController.ClearBoard();
        }
    }

    public void LoadLevel(int levelToLoad)
    {
        if (levelToLoad == 1)
        {
            SaveFile save = CoreGameElements.i.gameSave;
            StartMenu.SetStartTextStatic(save.firstRun);
            save.firstRun = false;
        }
        currentLevel = levelToLoad;
        boardController.firstTimeSetup = false;

        SaveFile.Board currentBoard =
            CoreGameElements.i.gameSave.boards[levelToLoad - 1];

        boardController.rowsMin = currentBoard.rowsMins;
        boardController.rowsMax = currentBoard.rowsMax;
        boardController.columsMin = currentBoard.columsMin;
        boardController.columnsMax = currentBoard.columnsMax;
        boardController.propCount.minimum = currentBoard.propsMin;
        boardController.propCount.maximum = currentBoard.propsMax;

        if (defaultGameSeed == "")
            defaultGameSeed = CoreGameElements.i.gameSave.defaultGameSeed;

        string levelName = defaultGameSeed + levelToLoad;
        currentSeed = levelName.GetHashCode();
        Random.InitState (currentSeed);
        boardController.SetupScene (levelToLoad);
        FXController
            .SetAnimatorTrigger_Static(FXController.Animations.LevelFader,
            "Fade");
        UIController
            .UpdateTextUI(UIController.UITextComponents.levelText,
            "Level " + levelToLoad);

        int coins = CoreGameElements.i.gameSave.playerCoins;
        CurrencyController.totalCoinsCollected = coins;
        ScoreDisplayUpdater
            .StartRoutineDown(coins, UIController.UITextComponents.coinText);
    }
}
