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
        multiplyText
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
        outerBeat
    }

    private static int heartIndex = 0;

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
                if (hearts[heartIndex].fillAmount > 0)
                    hearts[heartIndex].fillAmount -= heartValue;
                else if (heartIndex < hearts.Length - 1)
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
}
