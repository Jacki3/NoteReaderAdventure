using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using Unity.Mathematics;

public class NotePopup : MonoBehaviour
{
    public float floatSpeed;

    public float dissapearSpeed;

    public static NotePopup Create(Vector3 pos, string note, Color textColor)
    {
        NotePopup notePopup =
            Instantiate(CoreGameElements.i.notePopup, pos, Quaternion.identity);

        notePopup.Setup (note, textColor);

        return notePopup;
    }

    private TextMeshPro textMesh;

    private float dissapearTimer;

    private Color textColour;

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    private void Setup(string text, Color color)
    {
        textMesh.SetText (text);
        textMesh.color = color;
        textColour = textMesh.color;
        dissapearTimer = 1f;
    }

    void Update()
    {
        transform.position += new Vector3(0, floatSpeed) * Time.deltaTime;

        dissapearTimer -= Time.deltaTime;
        if (dissapearTimer < 0)
        {
            textColour.a -= dissapearSpeed * Time.deltaTime;
            textMesh.color = textColour;

            if (textColour.a < 0) Destroy(gameObject);
        }
    }
}
