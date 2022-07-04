using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public CoreUIElements.Text toolTipText;

    public RectTransform backgroundImageRect;

    public float toolTipLifeTime = 3;

    private float paddingSize;

    private Vector3 scale;

    private static Tooltip instance;

    private float spawnTime;

    private float lifeTime = 0;

    private Vector3 ascenionRate = Vector3.up;

    //NO - use UI Core Elements
    private Image backgroundImage;

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
        paddingSize = toolTipText.text.rectTransform.anchoredPosition.x;
        backgroundImage = backgroundImageRect.GetComponent<Image>();
        scale = transform.localScale;
    }

    private void Update()
    {
        var progress = (Time.time - spawnTime) / lifeTime;

        if (progress < 1)
        {
            gameObject.transform.position += ascenionRate * Time.deltaTime;

            var textColour = toolTipText.text.color;
            var imageColour = backgroundImage.color;
            textColour.a = 1 - progress;
            imageColour.a = 1 - progress;
            toolTipText.text.color = textColour;
            backgroundImage.color = imageColour;
        }
    }

    //add a 'warning' version of this
    public void SetToolTip(string text, Vector3 position, Transform parent)
    {
        if (parent == null) parent = transform.root;
        transform.SetParent (parent);

        spawnTime = Time.time;
        lifeTime = toolTipLifeTime;

        gameObject.SetActive(true);
        transform.localPosition = position;
        toolTipText.text.text = text;
        Vector2 backgroundSize =
            new Vector2(toolTipText.text.preferredWidth + paddingSize * 2,
                toolTipText.text.preferredHeight + paddingSize * 2);

        transform.localScale = scale;

        backgroundImageRect.sizeDelta = backgroundSize;
    }

    public static void SetToolTip_Static(
        string text,
        Vector3 position,
        Transform parent
    )
    {
        instance.SetToolTip (text, position, parent);
    }

    public void SetToolTipSkill(string text, Vector3 position, Transform parent)
    {
        if (parent == null) parent = transform.root;
        transform.SetParent (parent);

        spawnTime = Time.time;
        lifeTime = 60;

        gameObject.SetActive(true);
        transform.localPosition = position;
        toolTipText.text.text = text;
        toolTipText.text.color = Color.white;
        Vector2 backgroundSize =
            new Vector2(toolTipText.text.preferredWidth + paddingSize * 2,
                toolTipText.text.preferredHeight + paddingSize * 2);

        transform.localScale = scale;

        backgroundImageRect.sizeDelta = backgroundSize;
    }

    public static void SetToolTipSkill_Static(
        string text,
        Vector3 position,
        Transform parent
    )
    {
        instance.SetToolTipSkill (text, position, parent);
    }
}
