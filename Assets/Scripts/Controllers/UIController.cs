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
        skillPointText
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
        int maxFill,
        int valueToAdd
    )
    {
        CoreUIElements.i.GetSliderComponent(sliderComponent).maxValue = maxFill;
        CoreUIElements.i.GetSliderComponent(sliderComponent).value = valueToAdd;
    }

    public static string
    getBetween(string strSource, string strStart, string strEnd)
    {
        if (strSource.Contains(strStart) && strSource.Contains(strEnd))
        {
            int
                Start,
                End;
            Start = strSource.IndexOf(strStart, 0) + strStart.Length;
            End = strSource.IndexOf(strEnd, Start);
            return strSource.Substring(Start, End - Start);
        }

        return "";
    }
}
