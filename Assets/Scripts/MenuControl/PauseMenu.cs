using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
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

    public TMPro.TMP_ColorGradient colorGradient;

    public TMPro.TMP_ColorGradient colorGradientWhite;

    [Header("Audio")]
    public float audioFadeDur;

    public float musicVolTarget;

    public float menuVolTarget;

    public AudioMixer mixer;

    public AudioSource musicAudio;

    public AudioSource metroSource;

    public GameObject background;

    private bool optionsVisible;

    private Animator animator;

    private float currentMusicVol;

    private bool isMainMenu = true;

    private bool popUpVisible;

    void Start()
    {
        animator = GetComponent<Animator>();
        PlayerController.inputActions.Player.Escape.performed += ctx =>
            ShowMenu();

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

    public void ReturnToMain()
    {
        EventSystem.current.SetSelectedGameObject (mainMenuStart);

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
        LevelController.i.DelayResetPlayer();

        metroSource.enabled = false;
        StartCoroutine(FadeStartMenuAudio(audioFadeDur, 9, -80));
        //save player stats
    }

    public void ShowMenu()
    {
        if (GameStateController.state != GameStateController.States.MainMenu)
        {
            GameStateController.PauseGame(false);

            if (GameStateController.gamePaused)
            {
                GetComponent<Canvas>().enabled = true;
                optionButtons.SetActive(false);
                pauseButtons.SetActive(true);
                rhythmBar.SetActive(false);
                EventSystem.current.SetSelectedGameObject (resumeButton);
                mixer.GetFloat("MusicVol", out currentMusicVol);
                mixer.SetFloat("MusicVol", currentMusicVol - 8);
            }
            else
            {
                mixer.GetFloat("MusicVol", out currentMusicVol);
                mixer.SetFloat("MusicVol", currentMusicVol + 8);
                rhythmBar.SetActive(true);
                GetComponent<Canvas>().enabled = false;
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
        mixer.SetFloat("MusicVol", (Mathf.Log10(value) * 20) - 10);
        mixer.SetFloat("MainMenuVol", Mathf.Log10(value) * 20);
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
        isMainMenu = false;
        background.SetActive(true);
        pauseButtons.SetActive(true);
        musicAudio.Play();
        metroSource.enabled = true;
        StartCoroutine(FadeStartMenuAudio(audioFadeDur,
        menuVolTarget,
        musicVolTarget));
    }

    private IEnumerator
    FadeStartMenuAudio(float duration, float targetVolMenu, float targetVolGame)
    {
        GetComponent<Canvas>().enabled = isMainMenu ? true : false;

        float currentTime = 0;
        float currentVolMenu;
        float currentVolGame;
        mixer.GetFloat("MainMenuVol", out currentVolMenu);
        mixer.GetFloat("MusicVol", out currentVolGame);
        currentVolMenu = Mathf.Pow(10, currentVolMenu / 20);
        currentVolGame = Mathf.Pow(10, currentVolGame / 20);
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
            mixer.SetFloat("MainMenuVol", Mathf.Log10(newVolMenu) * 20);
            mixer.SetFloat("MusicVol", Mathf.Log10(newVolGame) * 20);
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
            Debug.Log("No save exists!");
    }

    private void ButtonClickSound()
    {
        SoundController.PlaySound(SoundController.Sound.ButtonClick);
    }
}
