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

    void Start()
    {
        if (!PlayerPrefs.HasKey("FirstPlay"))
        {
            Debug.Log("First Run!");
            PlayerPrefs.DeleteAll();
            int totalLevels = CoreGameElements.i.totalLevels;

            PlayerPrefs.SetInt("FirstPlay", 1);
            GenerateAllLevels (totalLevels);
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.H))
        {
            GenerateAllLevels(1);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadLevel(1);
        }
    }

    public void GenerateAllLevels(int totalLevels)
    {
        for (int i = 0; i < totalLevels; i++)
        {
            gameSeed = "TestLvlsBuild10_" + level;
            currentSeed = gameSeed.GetHashCode();
            Random.InitState (currentSeed);
            boardController.SetupScene (level);
            defaultGameSeed = gameSeed;
            PlayerPrefs.SetString("DefaultSeed", defaultGameSeed);

            PlayerPrefs.SetInt("testRowsMin" + level, boardController.rowsMin);
            PlayerPrefs.SetInt("testRowsMax" + level, boardController.rowsMax);
            PlayerPrefs
                .SetInt("testColumnsMin" + level, boardController.columsMin);
            PlayerPrefs
                .SetInt("testColumnsMax" + level, boardController.columnsMax);
            PlayerPrefs
                .SetInt("testPropsMin" + level,
                boardController.propCount.minimum);
            PlayerPrefs
                .SetInt("testPropsMax" + level,
                boardController.propCount.maximum);

            level++;

            if (i == totalLevels - 1) boardController.ClearBoard();
        }
    }

    public void LoadLevel(int levelToLoad)
    {
        currentLevel = levelToLoad;
        boardController.firstTimeSetup = false;

        boardController.rowsMin =
            PlayerPrefs.GetInt("testRowsMin" + levelToLoad);
        boardController.rowsMax =
            PlayerPrefs.GetInt("testRowsMax" + levelToLoad);
        boardController.columsMin =
            PlayerPrefs.GetInt("testColumnsMin" + levelToLoad);
        boardController.columnsMax =
            PlayerPrefs.GetInt("testColumnsMax" + levelToLoad);
        boardController.propCount.minimum =
            PlayerPrefs.GetInt("testPropsMin" + levelToLoad);
        boardController.propCount.maximum =
            PlayerPrefs.GetInt("testPropsMax" + levelToLoad);

        if (defaultGameSeed == null)
            defaultGameSeed = PlayerPrefs.GetString("DefaultSeed");

        string levelName = defaultGameSeed + levelToLoad;
        currentSeed = levelName.GetHashCode();
        Random.InitState (currentSeed);
        boardController.SetupScene (levelToLoad);
    }
}
