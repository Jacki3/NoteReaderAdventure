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

    [Tooltip("After how many levels should difficulty change?")]
    public int difficultyChangeLevel; //after how many levels should the difficulty change

    public int difficultyChangeLevelNotes; //after how many levels should the difficulty change

    public int totalCollectiblesThisLevel;

    public int totalCoinsCollected;

    public int scoreForRhythm;

    public bool useColours;

    public bool useBassNotes;

    public bool unlockAllLevels = false;

    public float chanceOfBassNotes = .65f;

    public bool useIntro = true;

    public bool useTutorial = true;

    public Notes allNotes;

    [Serializable]
    public class Notes
    {
        public int[] oneNotes;

        public int[] twoNotes;

        public int[] threeNotes;

        public int[] fourNotes;

        public int[] fiveNotes;

        public int[] sixNotes;

        public int[] sevenNotes;

        public int[] eightNotes;

        public int[] nineNotes;

        public int[] tenNotes;

        public int[] elevenNotes;

        public int[] twelveNotes;

        public int[] thirteenNotes;

        public int[] fourteenNotes;

        public int[] fifteenNotes;

        public int[] sixteenNotes;

        public int[] seventeenNotes;
    }

    public Patterns allPatterns;

    [Serializable]
    public class Patterns
    {
        public Pattern[] onePatterns;

        public Pattern[] twoPatterns;

        public Pattern[] threePatterns;

        public Pattern[] fourPatterns;

        public Pattern[] fivePatterns;

        public Pattern[] sixPatterns;

        public Pattern[] sevenPatterns;

        public Pattern[] eightPatterns;

        public Pattern[] ninePatterns;

        public Pattern[] tenPatterns;

        public Pattern[] elevenPatterns;

        public Pattern[] twelvePatterns;

        public Pattern[] thirteenPatterns;

        public Pattern[] fourteenPatterns;

        public Pattern[] fifteenPatterns;

        public Pattern[] sixteenPatterns;

        public Pattern[] seventeenPatterns;
    }

    public CameraShakes cameraShakes;

    public Difficuties currentDifficulty;

    public DifficutiesNotes currentDifficultyNotes;

    public enum Difficuties
    {
        absoluteBeginner,
        veryEasy,
        easy,
        medium,
        intermediate,
        hard,
        veryHard,
        ultraHard,
        superHard,
        hardest
    }

    public enum DifficutiesNotes
    {
        one,
        two,
        three,
        four,
        five,
        six,
        seven,
        eight,
        nine,
        ten,
        eleven,
        twelve,
        thirteen,
        fourteen,
        fifteen,
        sixteen,
        seventeen
    }

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
