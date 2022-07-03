using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public GameObject firstButton;

    public GameObject levelOneButton;

    public GameObject mainButtons;

    public GameObject optionButtons;

    public GameObject levelSelect;

    public Canvas gameCanvas;

    public Canvas pauseMenu;

    public TMPro.TextMeshProUGUI startText;

    public Seed levelGen;

    public LevelButton button;

    public Transform lvlSelectContent;

    public GameObject player;

    public PuzzleController puzzleController;

    private bool optionsVisible;

    private bool levelsVisible;

    private List<LevelButton> levelButtons = new List<LevelButton>();

    private static StartMenu i;

    private void Awake()
    {
        i = this;
        GameStateController.PauseGame(false);
        gameCanvas.enabled = false;
    }

    void Start()
    {
        EventSystem.current.SetSelectedGameObject (firstButton);

        //should avoid creating buttons everytime?
        int totalLevels = CoreGameElements.i.totalLevels;

        for (int i = 0; i < totalLevels; i++)
        {
            //Create a new button
            LevelButton newButton =
                Instantiate(button, Vector3.zero, Quaternion.identity);

            if (i == 0) levelOneButton = newButton.gameObject;

            levelButtons.Add (newButton);

            //Set load level to index
            int level = i + 1;
            var interactabeButton =
                newButton.GetComponent<UnityEngine.UI.Button>();
            interactabeButton
                .onClick
                .AddListener(delegate ()
                {
                    levelGen.LoadLevel (level);
                    StartGame();
                });

            //Set size, level/score text and parent
            Vector3 size = newButton.transform.localScale;
            newButton.transform.SetParent (lvlSelectContent);
            newButton.transform.localScale = size;
            string lvlName = (i + 1).ToString();
            newButton.SetLevelText (lvlName);
        }

        List<Button> allButtons = new List<Button>();
        transform.GetComponentsInChildrenRecursively<Button> (allButtons);

        foreach (Button button in allButtons)
        {
            button
                .onClick
                .AddListener(delegate ()
                {
                    ButtonClickSound();
                });
        }
    }

    public static void SetStartTextStatic(bool firstRun)
    {
        i.SetStartText (firstRun);
    }

    private void SetStartText(bool firstRun)
    {
        if (firstRun)
            startText.text = "new game";
        else
            startText.text = "continue";
    }

    public void StartGame()
    {
        GameStateController.state = GameStateController.States.Play;
        levelSelect.SetActive(false);
        mainButtons.SetActive(true);
        levelsVisible = false;
        gameObject.SetActive(false);
        pauseMenu.GetComponent<PauseMenu>().MusicFade();
        player.SetActive(true);

        gameCanvas.enabled = true;
        if (GameStateController.gamePaused) GameStateController.PauseGame(true);
    }

    public void ContinueGame()
    {
        int levelAt = CoreGameElements.i.gameSave.levelAt;
        if (levelAt == 0) levelAt = 1;
        if (levelAt < CoreGameElements.i.latetstLevel)
            levelAt = CoreGameElements.i.latetstLevel;
        levelGen.LoadLevel (levelAt);
        StartGame();
    }

    public static void UpdateButtonsStatic()
    {
        i.UpdateLevelButtons();
    }

    public void UpdateLevelButtons()
    {
        int levelAt = CoreGameElements.i.gameSave.levelAt;
        if (levelAt == 0) levelAt = 1;

        //Saving a local int for latest level as saving require resets (this ensures players can go from game to menu) but you could just save now with json
        if (levelAt < CoreGameElements.i.latetstLevel)
            levelAt = CoreGameElements.i.latetstLevel;
        if (CoreGameElements.i.unlockAllLevels) levelAt = levelButtons.Count;
        for (int i = 0; i < levelButtons.Count; i++)
        {
            if (i < levelAt)
            {
                var interactabeButton =
                    levelButtons[i].GetComponent<UnityEngine.UI.Button>();
                interactabeButton.interactable = true;
            }

            SaveFile newSave = CoreGameElements.i.gameSave;
            var board = newSave.boards[i];
            int score = board.score;
            if (score > 0)
            {
                levelButtons[i].SetScoreText(score.ToString());

                if (board.HasMaxScore()) levelButtons[i].SetCrown();
            }
            else
                levelButtons[i].SetScoreText("???");
        }
    }

    public void ShowOptions()
    {
        if (optionsVisible)
        {
            EventSystem.current.SetSelectedGameObject (firstButton);
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

    public void ShowLevelSelect()
    {
        UpdateLevelButtons();

        if (levelsVisible)
        {
            EventSystem.current.SetSelectedGameObject (firstButton);
            levelSelect.SetActive(false);
            mainButtons.SetActive(true);
            levelsVisible = false;
        }
        else
        {
            EventSystem.current.SetSelectedGameObject (levelOneButton);
            levelSelect.SetActive(true);
            mainButtons.SetActive(false);
            levelsVisible = true;
        }
    }

    public void ShowPuzzle()
    {
        gameObject.SetActive(false);
        player.SetActive(false);
        puzzleController.GeneratePuzzle();

        GameStateController.state = GameStateController.States.Puzzle;
    }

    public void Quit()
    {
        GameStateController.Quit();
    }

    private void ButtonClickSound()
    {
        SoundController.PlaySound(SoundController.Sound.ButtonClick);
    }
}
