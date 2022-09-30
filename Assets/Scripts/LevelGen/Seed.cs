﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

using Random = UnityEngine.Random;

public class Seed : MonoBehaviour
{
    public string gameSeed = "Default";

    public int currentSeed;

    public int level = 1;

    public int currentLevel;

    public BoardController boardController;

    public LevelController levelController;

    public string defaultGameSeed;

    public bool usingCustomLvl;

    public int customLevelNum;

    public Light2D mainLights;

    public int[] customLevels;

    public LevelObjs[] customLevel;

    public static int totalNotations;

    [Serializable]
    public class LevelObjs
    {
        public string levelName;

        public GameObject levelObj;

        public bool scrollUnlocked;

        public int levelScore;

        public List<Notation> levelNotations = new List<Notation>();

        public bool AllNotationsComplete() => levelNotations.Count <= 0;

        public void RemoveNotationFromList(Transform pos)
        {
            foreach (Notation notation in levelNotations.ToList())
            {
                INotation notationInterface =
                    notation.notation.GetComponent<INotation>();

                if (notationInterface != null)
                {
                    if (
                        notationInterface.GetTransform().position ==
                        pos.position
                    )
                    {
                        levelNotations.Remove (notation);
                        int notationsLeft =
                            totalNotations - levelNotations.Count;
                        UIController
                            .UpdateTextUI(UIController
                                .UITextComponents
                                .notationsText,
                            notationsLeft + "/" + totalNotations);
                    }
                }
            }
        }
    }

    public void CollectScrollForLevel()
    {
        if (currentLevel != 1)
        {
            if (usingCustomLvl)
            {
                customLevel[currentLevel - 1].scrollUnlocked = true;
            }
        }
    }

    public void SetLevels()
    {
        int totalLevels = CoreGameElements.i.totalLevels;
        GenerateAllLevels (totalLevels);
    }

    public void GenerateAllLevels(int totalLevels)
    {
        for (int i = 0; i < totalLevels; i++)
        {
            SaveFile.Board newBoard = new SaveFile.Board();

            gameSeed = "LvlBuilder10_";
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
            newBoard.smashMin = boardController.smashableCount.minimum;
            newBoard.smashMax = boardController.smashableCount.maximum;
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
        if (levelToLoad == -1) levelToLoad = currentLevel + 1;
        if (levelToLoad == -2) levelToLoad = currentLevel;
        if (levelToLoad == 1)
        {
            SaveFile save = CoreGameElements.i.gameSave;
            StartMenu.SetStartTextStatic(save.firstRun);
            save.firstRun = false;
        }
        currentLevel = levelToLoad;

        mainLights.enabled = true;

        usingCustomLvl = false;

        string UILevelName = "Level " + levelToLoad;

        DifficultyPicker.ChooseDifficultyOnLevel (currentLevel);
        DifficultyPicker.ChooseDifficultyOnLevelNotes (currentLevel);

        ScoreController.ResetScoreStatic();

        HideCustomLevel();

        for (int i = 0; i < customLevels.Length; i++)
        {
            if (levelToLoad == customLevels[i])
            {
                usingCustomLvl = true;
                customLevelNum = i;
                UILevelName = customLevel[i].levelName;
                customLevel[i].levelObj.SetActive(true);
                levelController.ResetPlayerPos();
                if (i == 0)
                {
                    CoreGameElements.i.useTutorial = true;
                    GameStateController.state =
                        GameStateController.States.Tutorial;
                    TutorialManager.LoadTutorial();
                }
                else if (i == 1)
                {
                    //arena mode
                    CoreGameElements.i.useTutorial = false;
                    GameStateController.state = GameStateController.States.Play;
                    mainLights.enabled = false;
                }
                else
                {
                    CoreGameElements.i.useTutorial = false;
                    GameStateController.state = GameStateController.States.Play;
                    totalNotations = customLevel[i].levelNotations.Count;
                    MusicGenController.StartCustomLvlMusic_Static();
                }
            }
        }
        if (!usingCustomLvl)
        {
            usingCustomLvl = false;

            boardController.firstTimeSetup = false;

            SaveFile.Board currentBoard =
                CoreGameElements.i.gameSave.boards[levelToLoad - 1];

            boardController.rowsMin = currentBoard.rowsMins;
            boardController.rowsMax = currentBoard.rowsMax;
            boardController.columsMin = currentBoard.columsMin;
            boardController.columnsMax = currentBoard.columnsMax;
            boardController.propCount.minimum = currentBoard.propsMin;
            boardController.propCount.maximum = currentBoard.propsMax;
            boardController.smashableCount.minimum = currentBoard.smashMin;
            boardController.smashableCount.maximum = currentBoard.smashMax;

            if (defaultGameSeed == "")
                defaultGameSeed = CoreGameElements.i.gameSave.defaultGameSeed;

            string levelName = defaultGameSeed + levelToLoad;
            currentSeed = levelName.GetHashCode();
            Random.InitState (currentSeed);
            boardController.SetupScene (levelToLoad);

            levelController.ResetPlayerPos();

            //load music on levels which are procedureally generated
            bool isEnemyLevel = levelToLoad % 5 == 0 ? true : false;
            MusicGenController.RegenMusic (isEnemyLevel);

            //if we load a non custom level it will always be a non tutorial level
            GameStateController.state = GameStateController.States.Play;

            totalNotations = boardController.notations.Count;
        }

        FXController
            .SetAnimatorTrigger_Static(FXController.Animations.LevelFader,
            "Fade");
        UIController
            .UpdateTextUI(UIController.UITextComponents.levelText, UILevelName);
        HealthController.UpdateHealth();
        int coins = CoreGameElements.i.gameSave.playerCoins;
        CurrencyController.totalCoinsCollected = 0;
        CurrencyController.AddRemoveCoins(coins, true);
        MissionHolder.i.LoadAllMissionsFromSave();
        ExperienceController.SetXP();
        PlayerSkills.LoadAllSkills();
        ScoreDisplayUpdater
            .StartRoutineDown(coins, UIController.UITextComponents.coinText);
        ScoreDisplayUpdater
            .StartRoutineDown(coins,
            UIController.UITextComponents.shopCoinText);

        UIController
            .UpdateTextUI(UIController.UITextComponents.notationsText,
            "0/" + totalNotations);

        Metronome.UnMuteMetro();
    }

    public void HideCustomLevel()
    {
        foreach (LevelObjs customLevel in customLevel)
        {
            customLevel.levelObj.SetActive(false);
        }
    }

    public static T DeepCopy<T>(T item)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream stream = new MemoryStream();
        formatter.Serialize (stream, item);
        stream.Seek(0, SeekOrigin.Begin);
        T result = (T) formatter.Deserialize(stream);
        stream.Close();
        return result;
    }
}
