using UnityEngine;

public static class UIController
{
    public enum UITextComponents
    {
        scoreText,
        currentLvlText,
        currentXPText,
        XPToNextLvlText,
        missionText,
        coinText,
        skillPointText,
        skillMenuSkillPointText,
        collectibleText
        //etc.
    }

    public enum UIImageComponents
    {
        healthBar,
        XPBar,
        multiplierBar,
        hearts,
        coin
        //etc.
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
        Sprite sprite
    )
    {
        //reflection - get type of UI element
        //find exisisting element type and change value of text - could still apply
        CoreUIElements.i.GetImageComponent(imageComponent).sprite = sprite;
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
