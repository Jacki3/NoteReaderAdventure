using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject musicSlider;

    public GameObject mainMenuStart;

    public GameObject mainButtons;

    public GameObject pauseButtons;

    public GameObject optionButtons;

    public GameObject resumeButton;

    public GameObject optionsResume;

    public GameObject areYouSurePopup;

    public GameObject rhythmBar;

    public Canvas gameCanvas;

    public StartMenu mainMenu;

    public GameObject skillTree;

    public GameObject skillTreeFirstButton;

    public TMPro.TMP_ColorGradient colorGradient;

    public TMPro.TMP_ColorGradient colorGradientWhite;

    public GameObject background;

    public Button returnButton;

    public Button gameOverExitButton;

    public Button endScreenButton;

    public GameObject customMenu;

    public GameObject MIDIMenu;

    public GameObject shopCanvas;

    public GameObject heartsUI;

    public RigidPlayerController rigidPlayerControllerScript;

    public PlayerController playerControllerScript;

    public GameObject helpScreenFirstButton;

    public GameObject customMenuFirstButton;

    [Header("Audio")]
    public float audioFadeDur;

    public float musicVolTarget;

    public float menuVolTarget;

    public AudioMixer mixer;

    public AudioSource musicAudio;

    public AudioSource metroSource;

    private bool optionsVisible;

    private Animator animator;

    private float currentMusicVol;

    private bool isMainMenu = true;

    private bool popUpVisible;

    private bool customMenuVisible = false;

    private bool MIDIMenuVisible;

    private bool controlsRigid = true;

    void Awake()
    {
        ExperienceController.LevelUp += ShowSkillMenu;
    }

    void Start()
    {
        StartCoroutine(FadeStartMenuAudio(audioFadeDur, 9, -80));
        animator = GetComponent<Animator>();
        RigidPlayerController.inputActions.Player.Escape.performed += ctx =>
            ShowMenu();

        returnButton
            .onClick
            .AddListener(delegate ()
            {
                ReturnToMain(false, false);
            });

        gameOverExitButton
            .onClick
            .AddListener(delegate ()
            {
                ReturnToMain(false, false);
            });
        endScreenButton
            .onClick
            .AddListener(delegate ()
            {
                ReturnToMain(false, false);
            });

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

    public void ReturnToMain(bool fromPuzzle, bool fromShop)
    {
        RigidPlayerController.readingMode = false;
        EndScreens.HideScreensStatic();
        EventSystem.current.SetSelectedGameObject (mainMenuStart);
        TutorialManager.ResetTutorialStatic();

        int coins = CoreGameElements.i.gameSave.playerCoins;
        CurrencyController.totalCoinsCollected = 0;
        CurrencyController.AddRemoveCoins(coins, true);
        UIController
            .UpdateTextUI(UIController.UITextComponents.shopCoinText,
            coins.ToString());

        pauseButtons.SetActive(false);
        background.SetActive(false);
        gameCanvas.enabled = false;
        mainMenu.gameObject.SetActive(true);
        rhythmBar.SetActive(true);
        isMainMenu = true;

        StartMenu.SetStartTextStatic(CoreGameElements.i.gameSave.firstRun);

        mainMenu.UpdateLevelButtons();

        GameStateController.state = GameStateController.States.MainMenu;
        mainMenu.levelGen.boardController.ClearBoard();
        mainMenu.levelGen.HideCustomLevel();
        LevelController.i.DelayResetPlayer();

        metroSource.enabled = false;
        StartCoroutine(FadeStartMenuAudio(audioFadeDur, 9, -80));
        MusicGenController.DisableMusic();

        if (fromPuzzle) mainMenu.ShowPuzzle();
        if (fromShop)
        {
            heartsUI.transform.SetParent(shopCanvas.transform, false);
            mainMenu.ShowShop(false);
        }
    }

    public void ShowMenu()
    {
        if (
            GameStateController.state != GameStateController.States.MainMenu &&
            GameStateController.state != GameStateController.States.Shopping &&
            GameStateController.state != GameStateController.States.Puzzle
        )
        {
            if (GameStateController.state == GameStateController.States.Tutorial
            )
                TutorialManager
                    .CheckTutorialStatic(Tutorial.TutorialValidation.Menu);
            GameStateController.PauseGame(false);

            if (GameStateController.gamePaused)
            {
                GameStateController.state = GameStateController.States.Paused;
                GetComponent<Canvas>().enabled = true;
                background.SetActive(true);
                optionButtons.SetActive(false);
                pauseButtons.SetActive(true);
                rhythmBar.SetActive(false);
                EventSystem.current.SetSelectedGameObject (resumeButton);
                mixer.GetFloat("MusicVol", out currentMusicVol);
                mixer.SetFloat("MusicVol", currentMusicVol - 8);
                mixer.GetFloat("LSequencer", out currentMusicVol);
                mixer.SetFloat("LSequencer", currentMusicVol - 8);
                mixer.GetFloat("BSequencer", out currentMusicVol);
                mixer.SetFloat("BSequencer", currentMusicVol - 8);
                Metronome.MuteMetro();
            }
            else
            {
                mixer.GetFloat("MusicVol", out currentMusicVol);
                mixer.SetFloat("MusicVol", currentMusicVol + 8);
                mixer.GetFloat("LSequencer", out currentMusicVol);
                mixer.SetFloat("LSequencer", currentMusicVol + 8);
                mixer.GetFloat("BSequencer", out currentMusicVol);
                mixer.SetFloat("BSequencer", currentMusicVol + 8);
                rhythmBar.SetActive(true);
                GetComponent<Canvas>().enabled = false;
                if (CoreGameElements.i.useTutorial)
                    GameStateController.state =
                        GameStateController.States.Tutorial;
                else
                    GameStateController.state = GameStateController.States.Play;
                Metronome.UnMuteMetro();
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }

    public void ShowPlayerCustomisation()
    {
        if (mainMenu.index.IndexFilled())
        {
            if (customMenuVisible)
            {
                customMenuVisible = false;
                customMenu.SetActive(false);
                if (isMainMenu)
                {
                    mainButtons.SetActive(true);
                    EventSystem.current.SetSelectedGameObject (mainMenuStart);
                }
                else
                {
                    pauseButtons.SetActive(true);
                    EventSystem.current.SetSelectedGameObject (resumeButton); //repeats below - concat showmenu but avoid pausing game everytime
                }
            }
            else
            {
                customMenuVisible = true;
                customMenu.SetActive(true);
                if (isMainMenu)
                {
                    mainButtons.SetActive(false);
                    EventSystem.current.SetSelectedGameObject (
                        customMenuFirstButton
                    );
                }
                else
                {
                    pauseButtons.SetActive(false);
                    EventSystem.current.SetSelectedGameObject (resumeButton); //repeats below - concat showmenu but avoid pausing game everytime
                }
            }
        }
    }

    public void ShowMIDISetupMenu()
    {
        if (MIDIMenuVisible)
        {
            MIDIMenuVisible = false;
            MIDIMenu.SetActive(false);
            if (isMainMenu)
            {
                optionButtons.SetActive(true);
                EventSystem.current.SetSelectedGameObject (musicSlider);
            }
            else
            {
                optionButtons.SetActive(true);
                background.SetActive(true);
                EventSystem.current.SetSelectedGameObject (musicSlider); //repeats below - concat showmenu but avoid pausing game everytime
            }
        }
        else
        {
            MIDIMenuVisible = true;
            MIDIMenu.SetActive(true);
            if (isMainMenu)
            {
                optionButtons.SetActive(false);
            }
            else
            {
                optionButtons.SetActive(false);
                background.SetActive(false);
                EventSystem.current.SetSelectedGameObject (
                    helpScreenFirstButton
                ); //repeats below - concat showmenu but avoid pausing game everytime
            }
        }
    }

    public void ShowOptions(bool showMain)
    {
        if (optionsVisible)
        {
            optionButtons.SetActive(false);
            if (showMain)
            {
                if (isMainMenu)
                {
                    mainButtons.SetActive(true);
                    EventSystem.current.SetSelectedGameObject (mainMenuStart);
                }
                else
                {
                    pauseButtons.SetActive(true);
                    EventSystem.current.SetSelectedGameObject (resumeButton); //repeats above - concat showmenu but avoid pausing game everytime
                }
            }
            optionsVisible = false;
        }
        else
        {
            optionButtons.SetActive(true);
            EventSystem.current.SetSelectedGameObject (musicSlider);
            if (showMain)
            {
                if (isMainMenu)
                    mainButtons.SetActive(false);
                else
                    pauseButtons.SetActive(false);
            }
            optionsVisible = true;
        }
    }

    public void Return()
    {
        MIDIMenu.SetActive(false);
        optionButtons.SetActive(true);
        EventSystem.current.SetSelectedGameObject (musicSlider);
        optionsVisible = true;
    }

    public void SetColour(bool switchSetting)
    {
        if (switchSetting)
        {
            if (CoreGameElements.i.useColours)
            {
                CoreGameElements.i.useColours = false;
            }
            else
            {
                CoreGameElements.i.useColours = true;
            }
        }

        if (CoreGameElements.i.useColours)
            UIController
                .UpdateTextColour(UIController
                    .UITextComponents
                    .colourOptionText,
                colorGradient);
        else
            UIController
                .UpdateTextColour(UIController
                    .UITextComponents
                    .colourOptionText,
                colorGradientWhite);

        CoreGameElements.i.gameSave.usingColour = CoreGameElements.i.useColours;
    }

    public void ShowAreYouSure()
    {
        ShowOptions(false);
        if (popUpVisible)
        {
            popUpVisible = false;
            areYouSurePopup.SetActive(false);
        }
        else
        {
            popUpVisible = true;
            areYouSurePopup.SetActive(true);
        }
    }

    public void SetMusicLevel(float value)
    {
        // mixer.SetFloat("MainMenuVol", Mathf.Log10(value) * 20);
        mixer.SetFloat("MusicVol", (Mathf.Log10(value) * 20) - 10);
        mixer.SetFloat("MusicVol", (Mathf.Log10(value) * 20) - 8);
        mixer.SetFloat("LSequencer", Mathf.Log10(value) * 20 - 8);
        mixer.SetFloat("BSequencer", Mathf.Log10(value) * 20 - 8);
        CoreGameElements.i.gameSave.musicVol =
            CoreUIElements
                .i
                .GetSliderComponent(UIController.UIImageComponents.musicVolBar)
                .value;
    }

    public void SetSFXLevel(float value)
    {
        mixer.SetFloat("SFXVol", Mathf.Log10(value) * 20);
        CoreGameElements.i.gameSave.SFXVol =
            CoreUIElements
                .i
                .GetSliderComponent(UIController.UIImageComponents.SFXVolBar)
                .value;
    }

    public void SetKeysLevel(float value)
    {
        mixer.SetFloat("KeysVol", Mathf.Log10(value) * 20);
        mixer.SetFloat("KeysMoveVol", Mathf.Log10(value) * 20);
        CoreGameElements.i.gameSave.keysVol =
            CoreUIElements
                .i
                .GetSliderComponent(UIController.UIImageComponents.keyVolBar)
                .value;
    }

    public void SetMetroLevel(float value)
    {
        mixer.SetFloat("MetroVol", Mathf.Log10(value) * 20);
        CoreGameElements.i.gameSave.metroVol =
            CoreUIElements
                .i
                .GetSliderComponent(UIController.UIImageComponents.metroVolBar)
                .value;
    }

    public void SetAllSliders()
    {
        float musicVal = CoreGameElements.i.gameSave.musicVol;
        float sfxVal = CoreGameElements.i.gameSave.SFXVol;
        float keysVal = CoreGameElements.i.gameSave.keysVol;
        float metroVal = CoreGameElements.i.gameSave.metroVol;

        UIController
            .UpdateSliderAmount(UIController.UIImageComponents.musicVolBar,
            1,
            musicVal);
        UIController
            .UpdateSliderAmount(UIController.UIImageComponents.SFXVolBar,
            1,
            sfxVal);
        UIController
            .UpdateSliderAmount(UIController.UIImageComponents.keyVolBar,
            1,
            keysVal);
        UIController
            .UpdateSliderAmount(UIController.UIImageComponents.metroVolBar,
            1,
            metroVal);
    }

    public void Quit()
    {
        GameStateController.Quit();
    }

    public void MusicFade()
    {
        heartsUI.transform.SetParent(gameCanvas.transform, false);
        isMainMenu = false;
        background.SetActive(true);
        pauseButtons.SetActive(true);
        musicAudio.Play();
        metroSource.enabled = true;
        StartCoroutine(FadeStartMenuAudio(audioFadeDur,
        menuVolTarget,
        CoreGameElements.i.gameSave.musicVol));
    }

    private IEnumerator
    FadeStartMenuAudio(float duration, float targetVolMenu, float targetVolGame)
    {
        GetComponent<Canvas>().enabled = isMainMenu ? true : false;

        float currentTime = 0;
        float currentVolMenu;
        float currentVolGame;
        float LSequencerVol;
        float BSequencerVol;
        mixer.GetFloat("MainMenuVol", out currentVolMenu);
        mixer.GetFloat("MusicVol", out currentVolGame);
        mixer.GetFloat("LSequencer", out LSequencerVol);
        mixer.GetFloat("BSequencer", out BSequencerVol);
        currentVolMenu = Mathf.Pow(10, currentVolMenu / 20);
        currentVolGame = Mathf.Pow(10, currentVolGame / 20);
        LSequencerVol = Mathf.Pow(10, LSequencerVol / 20);
        BSequencerVol = Mathf.Pow(10, BSequencerVol / 20);
        float targetValueMenu = Mathf.Clamp(targetVolMenu, 0.0001f, 1);
        float targetValueGame = Mathf.Clamp(targetVolGame, 0.0001f, 1);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVolMenu =
                Mathf
                    .Lerp(currentVolMenu,
                    targetValueMenu,
                    currentTime / duration);
            float newVolGame =
                Mathf
                    .Lerp(currentVolGame,
                    targetValueGame,
                    currentTime / duration);
            float newVolL =
                Mathf
                    .Lerp(currentVolGame,
                    targetValueGame,
                    currentTime / duration);
            float newVolB =
                Mathf
                    .Lerp(currentVolGame,
                    targetValueGame,
                    currentTime / duration);

            mixer.SetFloat("MainMenuVol", Mathf.Log10(newVolMenu) * 20);
            mixer.SetFloat("MusicVol", Mathf.Log10(newVolGame) * 20);
            mixer.SetFloat("LSequencer", Mathf.Log10(newVolL) * 20);
            mixer.SetFloat("BSequencer", Mathf.Log10(newVolB) * 20);
            yield return null;
        }
        yield break;
    }

    private IEnumerator MusicFade(float duration, float targetVol)
    {
        float currentTime = 0;
        float currentVol;
        mixer.GetFloat("MusicVol", out currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(targetVol, 0.0001f, 1);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol =
                Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            mixer.SetFloat("MusicVol", Mathf.Log10(newVol) * 20);

            yield return null;
        }
        yield break;
    }

    public void ShowSkillMenu()
    {
        if (GameStateController.state != GameStateController.States.MainMenu)
        {
            GameStateController.PauseGame(false);

            if (GameStateController.gamePaused)
            {
                GameStateController.state = GameStateController.States.Paused;
                GetComponent<Canvas>().enabled = true;
                background.SetActive(false);
                skillTree.SetActive(true);
                optionButtons.SetActive(false);
                pauseButtons.SetActive(false);
                rhythmBar.SetActive(false);
                EventSystem.current.SetSelectedGameObject (resumeButton); //make this a skill button
                mixer.GetFloat("MusicVol", out currentMusicVol);
                mixer.SetFloat("MusicVol", currentMusicVol - 8);
                EventSystem.current.SetSelectedGameObject (
                    skillTreeFirstButton
                );
            }
        }
    }

    public void HideSkillMenu()
    {
        mixer.GetFloat("MusicVol", out currentMusicVol);
        mixer.SetFloat("MusicVol", currentMusicVol + 8);
        rhythmBar.SetActive(true);
        skillTree.SetActive(false);
        GetComponent<Canvas>().enabled = false;
        GameStateController.PauseGame(false);
        GameStateController.state = GameStateController.States.Play;
    }

    public void DeleteSave()
    {
        if (File.Exists(Application.dataPath + "/save.txt"))
        {
            File.Delete(Application.dataPath + "/save.txt");
            CoreGameElements.i.gameSave = null;
            CoreGameElements.i.saveDeleted = true;
            Application.LoadLevel(0);
        }
        else
            Debug.LogWarning("No save exists!");
    }

    public void SetPlayerControls(TextMeshProUGUI buttonText)
    {
        if (!controlsRigid)
        {
            buttonText.text = "movement mode: rhythm";
            controlsRigid = true;
            rigidPlayerControllerScript.enabled = true;
            playerControllerScript.enabled = false;
        }
        else
        {
            buttonText.text = "movement mode: hold";
            controlsRigid = false;
            rigidPlayerControllerScript.enabled = false;
            playerControllerScript.enabled = true;
        }
    }

    private void ButtonClickSound()
    {
        SoundController.PlaySound(SoundController.Sound.ButtonClick);
    }
}
