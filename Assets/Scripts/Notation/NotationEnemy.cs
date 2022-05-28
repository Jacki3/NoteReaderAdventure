using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotationEnemy : MonoBehaviour, INotation
{
    public int notationsToComplete = 1;

    public Mission.Object missionObject;

    public AudioClip attackSound;

    public AudioClip walkSound;

    public SoundController.Sound hurtSound;

    public SoundController.Sound deathSound;

    public int healthToRemoveFromPlayer;

    public AudioSource audioSource;

    public Animator animator;

    private int notationsCompleted = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamagable damagable = other.GetComponent<IDamagable>();
        if (damagable != null) Damage(damagable);
    }

    public virtual void Damage(IDamagable damagable)
    {
        damagable.Damage (healthToRemoveFromPlayer);
    }

    public virtual void PlayedCorrectNote()
    {
        TakeDamage();
    }

    public void NotationComplete()
    {
        SoundController.PlaySound(SoundController.Sound.NotationComplete);
        notationsCompleted++;
        if (notationsCompleted >= notationsToComplete)
            TriggerDeath();
        else
            TakeDamage();
    }

    public virtual void TakeDamage()
    {
        if (hurtSound != SoundController.Sound.None)
            SoundController.PlaySound(hurtSound);
        if (animator != null) animator.SetTrigger("Hurt");
    }

    public virtual void TriggerDeath()
    {
        MissionHolder.i.CheckValidMission (missionObject);
        if (deathSound != SoundController.Sound.None)
            SoundController.PlaySound(deathSound);
        if (animator != null) animator.SetTrigger("Die");
    }

    public void Die()
    {
        Destroy (gameObject);
    }

    public void AttackAnim()
    {
        if (animator != null) animator.SetBool("Attack", true);
        if (attackSound != null)
        {
            audioSource.clip = attackSound;
            audioSource.Play();
        }
    }

    public void WalkAnim()
    {
        if (animator != null) animator.SetBool("Attack", false);
        if (walkSound != null)
        {
            audioSource.clip = walkSound;
            audioSource.Play();
        }
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
