using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class ShaderExpose : MonoBehaviour
{
    [SerializeField]
    public float _myCustomFloat;

    private float __myCustomFloatActualValue;

    private Color defaultColor;

    private Color colorNoTrans;

    private void LateUpdate()
    {
        defaultColor = GetComponent<Image>().color;

        colorNoTrans =
            new Color(defaultColor.r, defaultColor.g, defaultColor.b, 0);

        GetComponent<Image>()
            .material
            .SetColor("_Color", colorNoTrans * _myCustomFloat);
    }
}
