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

    private SpriteRenderer gateRenderer;

    private void Awake()
    {
        gateRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (KeyHolder.ContainsKey(keyType))
            {
                KeyHolder.RemoveKey (keyType);
                SoundController.PlaySound (sound);
                if (XPToAdd > 0) ExperienceController.AddXP(XPToAdd);
                if (scoreToAdd > 0) ScoreController.AddScore_Static(scoreToAdd);
                MissionHolder.i.CheckValidMission (missionObject);
                UIController
                    .UpdateImageSprite(UIController.UIImageComponents.goldKey,
                    null,
                    false);
                Tooltip
                    .SetToolTip_Static("Used Golden Key!",
                    Vector3.zero,
                    mainCanvas);
                Invoke("OpenGate", 1);
            }
            else
            {
                SoundController.PlaySound(SoundController.Sound.DoorLocked);
                Tooltip
                    .SetToolTip_Static("Requires Golden Key!",
                    Vector3.zero,
                    mainCanvas);
            }
        }
    }

    public void OpenGate()
    {
        gateRenderer.sprite = openGate;
        gateCollider.enabled = false;
    }
}
