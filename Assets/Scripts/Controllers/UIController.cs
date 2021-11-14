using UnityEngine;

public static class UIController
{
    public enum UITextComponents
    {
        scoreText,
        currentLvlText,
        currentXPText,
        XPToNextLvlText,
        missionText
        //etc.
    }

    public enum UIImageComponents
    {
        healthBar,
        XPBar,
        multiplierBar,
        hearts
        //etc.
    }

    public static void UpdateTextUI(UITextComponents component, string text)
    {
        //reflection - get type of UI element
        //find exisisting element type and change value of text - could still apply
        CoreUIElements.i.GetTextComponent(component).text = text;
    }

    public static void UpdateSliderAmount(
        UIImageComponents sliderComponent,
        int maxFill,
        int valueToAdd
    )
    {
        CoreUIElements.i.GetSliderComponent(sliderComponent).maxValue = maxFill;
        CoreUIElements.i.GetSliderComponent(sliderComponent).value = valueToAdd;
    }
}
