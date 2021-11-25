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

    public int maxHealth = 12;

    public int criticalHealth = 3;

    public int lowHealth = 7;

    public int totalLives = 5;

    public int totalCollectiblesThisLevel;

    public int totalCoinsCollected;
}
