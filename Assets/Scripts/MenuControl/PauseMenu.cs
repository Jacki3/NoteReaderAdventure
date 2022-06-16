using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
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
    }

    public void ReturnToMain()
    {
        SoundController.PlaySound(SoundController.Sound.ButtonClick);

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
        SoundController.PlaySound(SoundController.Sound.ButtonClick);

        if (optionsVisible)
        {
            EventSystem.current.SetSelectedGameObject (resumeButton); //repeats above - concat showmenu but avoid pausing game everytime
            optionButtons.SetActive(false);
            if (showMain)
            {
                if (isMainMenu)
                    mainButtons.SetActive(true);
                else
                    pauseButtons.SetActive(true);
            }
            optionsVisible = false;
        }
        else
        {
            optionButtons.SetActive(true);
            EventSystem.current.SetSelectedGameObject (optionsResume);
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

    public void SetColour(TMPro.TextMeshProUGUI text)
    {
        SoundController.PlaySound(SoundController.Sound.ButtonClick);

        //save COLOUR
        if (CoreGameElements.i.useColours)
        {
            CoreGameElements.i.useColours = false;
            text.colorGradientPreset = colorGradientWhite;
        }
        else
        {
            CoreGameElements.i.useColours = true;
            text.colorGradientPreset = colorGradient;
        }
    }

    public void ShowAreYouSure()
    {
        SoundController.PlaySound(SoundController.Sound.ButtonClick);
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
        //save MUSIC LEVEL
        mixer.SetFloat("MusicVol", (Mathf.Log10(value) * 20) - 10);
        if (isMainMenu) mixer.SetFloat("MainMenuVol", Mathf.Log10(value) * 20);
    }

    public void SetSFXLevel(float value)
    {
        //save SFX LEVEL
        mixer.SetFloat("SFXVol", Mathf.Log10(value) * 20);
    }

    public void SetKeysLevel(float value)
    {
        //save KEYS LEVEL
        mixer.SetFloat("KeysVol", Mathf.Log10(value) * 20);
        mixer.SetFloat("KeysMoveVol", Mathf.Log10(value) * 20);
    }

    public void SetMetroLevel(float value)
    {
        //save METRO LEVEL
        mixer.SetFloat("MetroVol", Mathf.Log10(value) * 20);
    }

    public void Quit()
    {
        SoundController.PlaySound(SoundController.Sound.ButtonClick);
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
}
