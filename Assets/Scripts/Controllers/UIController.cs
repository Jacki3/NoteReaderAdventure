using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

public static class UIController
{
    public enum UITextComponents
    {
        scoreText,
        streakText,
        currentLvlText,
        currentXPText,
        XPToNextLvlText,
        missionText,
        coinText,
        skillPointText,
        skillMenuSkillPointText,
        collectibleText,
        livesText,
        arenaWinText,
        multiplyText,
        levelText,
        danceFloorText,
        colourOptionText
    }

    public enum UIImageComponents
    {
        healthBar,
        XPBar,
        multiplierBar,
        hearts,
        coin,
        goldKey,
        silverKey,
        ironKey,
        staminaBar,
        outerBeat,
        musicVolBar,
        SFXVolBar,
        metroVolBar,
        keyVolBar
    }

    private static int heartIndex = 0;

    private static int additionalHearts = 0;

    //this should be called set text?
    public static void UpdateTextUI(UITextComponents component, string text)
    {
        //reflection - get type of UI element
        //find exisisting element type and change value of text - could still apply
        if (CoreUIElements.i != null)
            CoreUIElements.i.GetTextComponent(component).text = text;
    }

    public static void AppendTextUI(UITextComponents component, string text)
    {
        //reflection - get type of UI element
        //find exisisting element type and change value of text - could still apply
        CoreUIElements.i.GetTextComponent(component).text += text;
    }

    public static void UpdateTextColour(
        UITextComponents component,
        TMP_ColorGradient colorGradient
    )
    {
        CoreUIElements.i.GetTextComponent(component).colorGradientPreset =
            colorGradient;
    }

    public static void UpdateImageSprite(
        UIImageComponents imageComponent,
        Sprite sprite,
        bool enable
    )
    {
        //reflection - get type of UI element
        //find exisisting element type and change value of text - could still apply
        CoreUIElements.i.GetImageComponent(imageComponent).sprite = sprite;
        if (enable)
            CoreUIElements.i.GetImageComponent(imageComponent).enabled = true;
        else
            CoreUIElements.i.GetImageComponent(imageComponent).enabled = false;
    }

    public static void UpdateSliderAmount(
        UIImageComponents sliderComponent,
        float maxFill,
        float valueToAdd
    )
    {
        CoreUIElements.i.GetSliderComponent(sliderComponent).maxValue = maxFill;
        CoreUIElements.i.GetSliderComponent(sliderComponent).value = valueToAdd;
    }

    public static void UpdateImageColour(
        CoreUIElements.ImageUI image,
        Color32 color
    )
    {
        image.image.color = color;
    }

    public static void UpdateHearts(float value, bool remove)
    {
        float heartValue = 1f / 4; // not 4 but acutal value
        var hearts = CoreUIElements.i.hearts;

        for (int i = 0; i < value; i++)
        {
            if (remove)
            {
                if (hearts[heartIndex].fillAmount > 0 && hearts[heartIndex])
                    hearts[heartIndex].fillAmount -= heartValue;
                else if (heartIndex < hearts.Count - 1)
                {
                    heartIndex++;
                    hearts[heartIndex].fillAmount -= heartValue;
                }
            }
            else
            {
                if (hearts[heartIndex].fillAmount < 1)
                    hearts[heartIndex].fillAmount += heartValue;
                else if (heartIndex > 0)
                {
                    heartIndex--;
                    hearts[heartIndex].fillAmount += heartValue;
                }
            }
        }
    }

    public static void ResetHearts()
    {
        var hearts = CoreUIElements.i.hearts;
        for (int i = 0; i < hearts.Count; i++)
        {
            heartIndex = 0;
            hearts[i].fillAmount = 1;
        }
    }

    public static void AddHeart(bool updateUI)
    {
        var hearts = CoreUIElements.i.hearts;
        var heart = CoreUIElements.i.heart;
        float firstHeartPosX = hearts[0].transform.parent.localPosition.x;
        float secondHeartPosX = hearts[1].transform.parent.localPosition.x;

        float distance = firstHeartPosX - secondHeartPosX;

        var heartContainer = hearts[hearts.Count - 1].transform.parent;

        var newHeart = Object.Instantiate(heart);

        newHeart.transform.SetParent (heartContainer);
        newHeart.transform.localScale = hearts[0].transform.localScale;
        newHeart.transform.localPosition =
            new Vector2(firstHeartPosX + distance, 0);

        var heartChild = newHeart.transform.GetChild(0);

        CoreUIElements.i.AddHeartToList (heartChild);
        if (!updateUI) additionalHearts++;
    }

    public static void SaveUIHearts()
    {
        CoreGameElements.i.gameSave.additionalHearts = additionalHearts;
    }

    public static void LoadUIHearts()
    {
        additionalHearts = CoreGameElements.i.gameSave.additionalHearts;
        for (int i = 0; i < additionalHearts; i++)
        {
            AddHeart(true);
        }
    }
}
