using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public Sprite openGate;

    public SoundController.Sound sound;

    public int XPToAdd;

    public int scoreToAdd;

    public Mission.Object missionObject;

    [SerializeField]
    private Key.KeyType keyType;

    public BoxCollider2D gateCollider;

    public Transform mainCanvas;

    public UIController.UIImageComponents keyImage;

    protected SpriteRenderer gateRenderer;

    public bool gateOpen = false;

    private void Awake()
    {
        gateRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!gateOpen)
        {
            if (other.tag == "Player")
            {
                TryGate();
            }
        }
    }

    public virtual void TryGate()
    {
        if (KeyHolder.ContainsKey(keyType))
        {
            KeyHolder.RemoveKey (keyType);
            if (XPToAdd > 0) ExperienceController.AddXP(XPToAdd);
            if (scoreToAdd > 0) ScoreController.AddScore_Static(scoreToAdd);
            MissionHolder.i.CheckValidMission (missionObject);
            UIController.UpdateImageSprite(keyImage, null, false);
            Tooltip
                .SetToolTip_Static("Used " + keyType + " Key!",
                Vector3.zero,
                mainCanvas);
            Invoke("OpenGate", 1);
        }
        else
        {
            SoundController.PlaySound(SoundController.Sound.DoorLocked);
            Tooltip
                .SetToolTip_Static("Requires " + keyType + " Key!",
                Vector3.zero,
                mainCanvas);
        }
    }

    public virtual void OpenGate()
    {
        gateOpen = true;
        SoundController.PlaySound (sound);
        gateRenderer.sprite = openGate;
        gateCollider.enabled = false;
    }
}
