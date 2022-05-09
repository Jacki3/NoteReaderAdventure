using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoreUIElements : MonoBehaviour
{
    private static CoreUIElements _i;

    public static CoreUIElements i
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

    public UIText[] textComponents;

    [System.Serializable]
    public class UIText
    {
        public static TextMeshProUGUI textMeshProUGUI;

        public UIController.UITextComponents textComponent;

        public TextMeshProUGUI textPlaceholder;
    }

    public UIImage[] imageComponents;

    [System.Serializable]
    public class UIImage
    {
        public UIController.UIImageComponents imageComponent;

        public Image image;

        public Slider slider;
    }

    public Image[] hearts;

    public TextMeshProUGUI
    GetTextComponent(UIController.UITextComponents textComponent)
    {
        foreach (UIText text in textComponents)
        {
            if (text.textComponent == textComponent)
                return text.textPlaceholder;
        }
        Debug.LogError("Text Component" + textComponent + "missing!");
        return null;
    }

    public Image
    GetImageComponent(UIController.UIImageComponents imageComponent)
    {
        foreach (UIImage image in imageComponents)
        {
            if (image.imageComponent == imageComponent) return image.image;
        }
        Debug.LogError("Text Component" + imageComponent + "missing!");
        return null;
    }

    //this and above should NOT be here!?
    public Slider
    GetSliderComponent(UIController.UIImageComponents sliderComponent)
    {
        foreach (UIImage slider in imageComponents)
        {
            if (slider.imageComponent == sliderComponent) return slider.slider;
        }
        Debug.LogError("Slider Component" + sliderComponent + "missing!");
        return null;
    }

    [System.Serializable]
    public class Text
    {
        public TextMeshProUGUI text;
    }

    [System.Serializable]
    public class ImageUI
    {
        public Image image;
    }
}
