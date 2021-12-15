using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public GameObject mainButtons;

    public GameObject optionButtons;

    public GameObject resumeButton;

    public TMPro.TMP_ColorGradient colorGradient;

    public TMPro.TMP_ColorGradient colorGradientWhite;

    [Header("Audio")]
    public AudioMixer mixer;

    private bool optionsVisible;

    public void ShowMenu()
    {
        EventSystem.current.SetSelectedGameObject (resumeButton);
        GameStateController.PauseGame();
        if (GameStateController.gamePaused)
        {
            this.gameObject.SetActive(true);
            optionButtons.SetActive(false);
            mainButtons.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    public void ShowOptions()
    {
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

    public void Quit()
    {
        GameStateController.Quit();
    }
}
