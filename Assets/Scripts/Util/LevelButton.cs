using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public TextMeshProUGUI levelText;

    public void SetLevelText(string text)
    {
        levelText.text = text;
    }

    public void SetScoreText()
    {
    }
}
