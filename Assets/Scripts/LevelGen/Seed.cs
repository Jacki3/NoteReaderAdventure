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

    private string defaultGameSeed;

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
            gameSeed = "LvlBuild_" + level;
            currentSeed = gameSeed.GetHashCode();
            Random.InitState (currentSeed);
            boardController.SetupScene (level);
            defaultGameSeed = gameSeed;

            SaveFile.Board newBoard = new SaveFile.Board();

            CoreGameElements.i.gameSave.defaultGameSeed = defaultGameSeed;

            newBoard.level = level;
            newBoard.rowsMins = boardController.rowsMin;
            newBoard.rowsMax = boardController.rowsMax;
            newBoard.columsMin = boardController.columsMin;
            newBoard.columnsMax = boardController.columnsMax;
            newBoard.propsMin = boardController.propCount.minimum;
            newBoard.propsMax = boardController.propCount.maximum;

            CoreGameElements.i.gameSave.boards.Add (newBoard);

            level++;

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
            CoreGameElements.i.gameSave.boards[levelToLoad];

        boardController.rowsMin = currentBoard.rowsMins;
        boardController.rowsMax = currentBoard.rowsMax;
        boardController.columsMin = currentBoard.columsMin;
        boardController.columnsMax = currentBoard.columnsMax;
        boardController.propCount.minimum = currentBoard.propsMin;
        boardController.propCount.maximum = currentBoard.propsMax;

        if (defaultGameSeed == null)
            defaultGameSeed = CoreGameElements.i.gameSave.defaultGameSeed;

        string levelName = defaultGameSeed + levelToLoad;
        currentSeed = levelName.GetHashCode();
        Random.InitState (currentSeed);
        boardController.SetupScene (levelToLoad);
    }
}
