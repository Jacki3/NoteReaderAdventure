using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : MonoBehaviour
{
    public AltarGate gate;

    public Transform mainCanvas;

    public Color targetColor;

    public float lerpSpeed;

    public List<SpriteRenderer> runes;

    private Color curColor;

    private Color litColor;

    private bool isUnlocked;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<StoneCube>() != null)
        {
            if (!isUnlocked)
            {
                isUnlocked = true;
                litColor = targetColor;
                gate.OpenGate();
                SoundController.PlaySound(SoundController.Sound.ChestOpen);
                Tooltip
                    .SetToolTip_Static("A Gate Has Opended Somewhere...",
                    Vector3.zero,
                    mainCanvas);
                other.GetComponent<StoneCube>().enabled = false;
            }
        }
    }

    private void Update()
    {
        curColor = Color.Lerp(curColor, litColor, lerpSpeed * Time.deltaTime);

        foreach (var r in runes)
        {
            r.color = curColor;
        }
    }
}
