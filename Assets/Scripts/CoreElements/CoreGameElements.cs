using System;
using UnityEngine;
using UnityEngine.Audio;

public class CoreGameElements : MonoBehaviour
{
    private static CoreGameElements _i;

    public static CoreGameElements i
    {
        get
        {
            return _i;
        }
    }

    private void Awake()
    {
        if (_i != null && _i != this)
            Destroy(this.gameObject);
        else
            _i = this;

        GameStateController.state = GameStateController.States.MainMenu;
    }

    void Start()
    {
        //this should be in your scene manager/game manager/state manager
        UIController
            .UpdateTextUI(UIController.UITextComponents.collectibleText,
            "0/" + totalCollectiblesThisLevel);
    }

    [System.Serializable]
    public class Pattern
    {
        public int[] pattern;
    }

    [System.Serializable]
    public class CameraShakeComps
    {
        public float

                magnitude,
                roughness,
                fadeInTime,
                fadeOutTime;
    }

    [System.Serializable]
    public class CameraShakes
    {
        public CameraShakeComps healthShake;

        public CameraShakeComps movementShake;
    }

    public SaveFile gameSave;

    public bool saveDeleted = false;

    public NotePopup notePopup;

    public Canvas mainCanvas;

    public AudioMixer SFXMixer;

    public int totalLevels;

    public int latetstLevel;

    public int maxHealth = 12;

    public int criticalHealth = 3;

    public int lowHealth = 7;

    public int totalLives = 5;

    public int totalCollectiblesThisLevel;

    public int totalCoinsCollected;

    public int scoreForRhythm;

    public bool useColours;

    public bool useBassNotes;

    public bool unlockAllLevels = false;

    public float chanceOfBassNotes = .65f;

    public bool useIntro = true;

    public bool useTutorial = true;

    public int[] notes;

    public int[] notesBass;

    public Pattern[] patterns;

    public Pattern[] patternsBass;

    public CameraShakes cameraShakes;

    [System.NonSerialized]
    public Color32[]
        noteColours =
        {
            new Color32(194, 0, 40, 255),
            new Color32(208, 64, 88, 255),
            new Color32(222, 128, 75, 255),
            new Color32(236, 191, 163, 255),
            new Color32(250, 255, 50, 255),
            new Color32(134, 192, 59, 255),
            new Color32(18, 128, 68, 255),
            new Color32(43, 134, 162, 255),
            new Color32(68, 140, 255, 255),
            new Color32(100, 105, 216, 255),
            new Color32(131, 70, 178, 255),
            new Color32(163, 35, 139, 255)
        };
}
