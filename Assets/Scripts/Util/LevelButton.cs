using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public TextMeshProUGUI levelText;

    public TextMeshProUGUI scoreText;

    public Color highScoreColour;

    public UnityEngine.UI.Image crownRenderer;

    public void SetLevelText(string text)
    {
        levelText.text = text;
    }

    public void SetScoreText(string text)
    {
        scoreText.text = text;
    }

    public void SetCrown()
    {
        crownRenderer.enabled = true;
        scoreText.color = highScoreColour;
    }
}
