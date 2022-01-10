using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotationItem : MonoBehaviour, INotation
{
    public int notationsToComplete = 1;

    public int XPToAdd;

    public int scoreToAdd;

    public Mission.Object missionObject;

    public SoundController.Sound sound;

    protected int notationsCompleted;

    public virtual void NotationComplete()
    {
        ExperienceController.AddXP (XPToAdd);
        if (scoreToAdd > 0) ScoreController.AddScore_Static(scoreToAdd);
        MissionHolder.i.CheckValidMission (missionObject);
        SoundController.PlaySound (sound);

        var notationFloating =
            transform.GetComponentInChildren<ParticleSystem>();

        if (notationFloating != null)
            notationFloating.gameObject.SetActive(false);
    }

    void OnBecameInvisible()
    {
        foreach (Transform child in transform)
        {
            Notation notation = child.GetComponent<Notation>();
            if (notation != null)
            {
                notation.objectShow = false;
                notation.HideNotation();
            }
        }
    }

    void OnBecameVisible()
    {
        if (PlayerController.readingMode)
        {
            foreach (Transform child in transform)
            {
                Notation notation = child.GetComponent<Notation>();
                if (notation != null)
                {
                    notation.objectShow = true;
                    notation.ShowNotation();
                }
            }
        }
    }
}
