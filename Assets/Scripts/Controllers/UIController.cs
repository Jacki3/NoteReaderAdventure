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
        arenaWinText
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
        ironKey
    }

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
}
