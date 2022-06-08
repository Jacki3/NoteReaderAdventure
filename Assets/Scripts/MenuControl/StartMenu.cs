using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public GameObject mainButtons;

    public GameObject optionButtons;

    public GameObject levelSelect;

    public Canvas gameCanvas;

    public Canvas pauseMenu;

    public Animator menuAnimator;

    public Seed levelGen;

    public LevelButton button;

    public Transform lvlSelectContent;

    private bool optionsVisible;

    private bool levelsVisible;

    public List<LevelButton> levelButtons = new List<LevelButton>();

    private void Awake()
    {
        GameStateController.PauseGame(false);
        gameCanvas.enabled = false;
    }

    void Start()
    {
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
            newButton.transform.localScale = size;
            string lvlName = (i + 1).ToString();
            newButton.SetLevelText (lvlName);
        }
    }

    public void StartGame()
    {
        GameStateController.state = GameStateController.States.Play;
        levelSelect.SetActive(false);
        mainButtons.SetActive(true);
        levelsVisible = false;
        gameObject.SetActive(false);
        FXController
            .SetAnimatorTrigger_Static(FXController.Animations.LevelFader,
            "Fade");
        pauseMenu.GetComponent<PauseMenu>().MusicFade();

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

    public void UpdateLevelButtons()
    {
        int levelAt = CoreGameElements.i.gameSave.levelAt;
        if (levelAt == 0) levelAt = 1;

        //Saving a local int for latest level as player prefs require resets (this ensures players can go from game to menu)

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
        }
    }

    public void ShowOptions()
    {
        SoundController.PlaySound(SoundController.Sound.ButtonClick);
        if (optionsVisible)
        {
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
        SoundController.PlaySound(SoundController.Sound.ButtonClick);
        UpdateLevelButtons();

        if (levelsVisible)
        {
            levelSelect.SetActive(false);
            mainButtons.SetActive(true);
            levelsVisible = false;
        }
        else
        {
            levelSelect.SetActive(true);
            mainButtons.SetActive(false);
            levelsVisible = true;
        }
    }

    public void Quit()
    {
        SoundController.PlaySound(SoundController.Sound.ButtonClick);
        GameStateController.Quit();
    }
}
