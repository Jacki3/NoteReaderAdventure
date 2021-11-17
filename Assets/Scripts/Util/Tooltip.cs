using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public CoreUIElements.Text toolTipText;

    public RectTransform backgroundImage;

    private float paddingSize;

    private static Tooltip instance;

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
        paddingSize = toolTipText.text.rectTransform.anchoredPosition.x;
    }

    public void SetToolTip(string text, Vector3 position)
    {
        gameObject.SetActive(true);
        transform.localPosition = position;
        toolTipText.text.text = text;
        Vector2 backgroundSize =
            new Vector2(toolTipText.text.preferredWidth + paddingSize * 2,
                toolTipText.text.preferredHeight + paddingSize * 2);

        backgroundImage.sizeDelta = backgroundSize;

        //start a thing to fade it out
    }

    public static void SetToolTip_Static(string text, Vector3 position)
    {
        instance.SetToolTip (text, position);
    }
}
