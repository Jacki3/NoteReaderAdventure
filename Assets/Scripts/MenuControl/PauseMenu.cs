using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public GameObject mainButtons;

    public GameObject optionButtons;

    public GameObject resumeButton;

    public GameObject optionsResume;

    public GameObject rhythmBar;

    public TMPro.TMP_ColorGradient colorGradient;

    public TMPro.TMP_ColorGradient colorGradientWhite;

    [Header("Audio")]
    public AudioMixer mixer;

    private bool optionsVisible;

    public void ShowMenu()
    {
        GameStateController.PauseGame();
        if (GameStateController.gamePaused)
        {
            EventSystem.current.SetSelectedGameObject (resumeButton);
            this.gameObject.SetActive(true);
            optionButtons.SetActive(false);
            mainButtons.SetActive(true);
            rhythmBar.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(false);
            rhythmBar.SetActive(true);
        }
    }

    public void ShowOptions()
    {
        if (optionsVisible)
        {
            EventSystem.current.SetSelectedGameObject (resumeButton); //repeats above - concat showmenu but avoid pausing game everytime
            optionButtons.SetActive(false);
            mainButtons.SetActive(true);
            optionsVisible = false;
        }
        else
        {
            optionButtons.SetActive(true);
            EventSystem.current.SetSelectedGameObject (optionsResume);
            mainButtons.SetActive(false);
            optionsVisible = true;
        }
    }

    public void SetColour(TMPro.TextMeshProUGUI text)
    {
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

    public void SetMusicLevel(float value)
    {
        mixer.SetFloat("MusicVol", Mathf.Log10(value) * 20);
    }

    public void SetSFXLevel(float value)
    {
        mixer.SetFloat("SFXVol", Mathf.Log10(value) * 20);
    }

    public void SetKeysLevel(float value)
    {
        mixer.SetFloat("KeysVol", Mathf.Log10(value) * 20);
        mixer.SetFloat("KeysMoveVol", Mathf.Log10(value) * 20);
    }

    public void SetMetroLevel(float value)
    {
        mixer.SetFloat("MetroVol", Mathf.Log10(value) * 20);
    }

    public void Quit()
    {
        GameStateController.Quit();
    }
}
