using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public UserIndex index;

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

    public RigidPlayerController player;

    public PuzzleController puzzleController;

    public ItemShopController itemShop;

    public GameObject[] worldButtons;

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
            newButton.transform.localScale = size; //pretty sure this can be avoided with overloads of above method?
            string lvlName = (i + 1).ToString();
            int[] customLevels = levelGen.customLevels;
            for (int j = 0; j < customLevels.Length; j++)
            {
                if (i + 1 == customLevels[j])
                {
                    lvlName = levelGen.customLevel[j].levelName;
                }
            }
            newButton.SetLevelText (lvlName);
            newButton.gameObject.SetActive(false);
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

    public void ShowLevels(int maxLevel)
    {
        int minLevel = maxLevel - 10;

        foreach (GameObject worldButton in worldButtons)
        worldButton.SetActive(false);

        foreach (LevelButton button in levelButtons)
        button.gameObject.SetActive(false);

        for (int i = minLevel; i < maxLevel; i++)
        {
            levelButtons[i].gameObject.SetActive(true);

            if (i == minLevel)
                EventSystem
                    .current
                    .SetSelectedGameObject(levelButtons[i].gameObject);
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
        ShowLevelSelect();
        levelSelect.SetActive(false);
        mainButtons.SetActive(true);
        levelsVisible = false;
        gameObject.SetActive(false);
        pauseMenu.GetComponent<PauseMenu>().MusicFade();
        player.HidePlayer(false);

        gameCanvas.enabled = true;
        if (GameStateController.gamePaused)
            GameStateController.PauseGame(false);
    }

    public void ContinueGame()
    {
        if (index.IndexFilled())
        {
            int levelAt = CoreGameElements.i.gameSave.levelAt;
            if (levelAt == 0) levelAt = 1;
            if (levelAt < CoreGameElements.i.latetstLevel)
                levelAt = CoreGameElements.i.latetstLevel;
            levelGen.LoadLevel (levelAt);
            StartGame();
        }
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
        if (index.IndexFilled())
        {
            UpdateLevelButtons();

            if (levelsVisible)
            {
                EventSystem.current.SetSelectedGameObject (firstButton);
                levelSelect.SetActive(false);
                mainButtons.SetActive(true);
                levelsVisible = false;
                foreach (GameObject worldButton in worldButtons)
                worldButton.SetActive(true);
                foreach (LevelButton button in levelButtons)
                button.gameObject.SetActive(false);
            }
            else
            {
                levelOneButton = worldButtons[0];
                EventSystem.current.SetSelectedGameObject (levelOneButton);
                levelSelect.SetActive(true);
                mainButtons.SetActive(false);
                levelsVisible = true;
            }
        }
    }

    public void ShowPuzzle()
    {
        gameObject.SetActive(false);
        player.HidePlayer(true);
        puzzleController.gameObject.SetActive(true);
        puzzleController.GeneratePuzzle();

        GameStateController.state = GameStateController.States.Puzzle;
    }

    public void ShowShop()
    {
        gameObject.SetActive(false);
        player.HidePlayer(true);
        itemShop.ShowShop(player.GetComponent<IShopCustomer>());
        GameStateController.state = GameStateController.States.Shopping;
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
