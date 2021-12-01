using UnityEngine;

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

    public int maxHealth = 12;

    public int criticalHealth = 3;

    public int lowHealth = 7;

    public int totalLives = 5;

    public int totalCollectiblesThisLevel;

    public int totalCoinsCollected;

    public bool useColours;

    public int[] notes;

    public Pattern[] patterns;

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
