using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotationItem : MonoBehaviour, INotation
{
    public int notationsToComplete = 1;

    public int XPToAdd;

    public int scoreToAdd;

    public Animator animator;

    public Mission.Object missionObject;

    public SoundController.Sound sound;

    protected int notationsCompleted;

    public void PlayedCorrectNote()
    {
        //do something when correct note is played
    }

    public void NotationComplete()
    {
        LevelController.i.levelLoader.boardController.RemoveNotationFromList (
            transform
        );

        notationsCompleted++;
        if (notationsCompleted >= notationsToComplete)
            AllNotationsComplete();
        else
            DamageItem();
    }

    protected virtual void DamageItem()
    {
        SoundController.PlaySound(SoundController.Sound.NotationComplete);
        if (animator != null) animator.SetTrigger("Hurt");
    }

    protected virtual void AllNotationsComplete()
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

    public Transform GetTransform()
    {
        return transform;
    }
}
