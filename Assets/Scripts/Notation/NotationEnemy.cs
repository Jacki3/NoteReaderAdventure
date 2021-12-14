using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotationEnemy : MonoBehaviour, INotation
{
    public int notationsToComplete = 1;

    public Mission.Object missionObject;

    public AudioClip deathSound;

    public AudioClip attackSound;

    public AudioClip walkSound;

    public int healthToRemoveFromPlayer;

    private Animator animator;

    private AudioSource audioSource;

    private int notationsCompleted = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamagable damagable = other.GetComponent<IDamagable>();
        if (damagable != null) damagable.Damage(healthToRemoveFromPlayer);
    }

    public void NotationComplete()
    {
        notationsCompleted++;
        if (notationsCompleted >= notationsToComplete) TriggerDeath();
    }

    public virtual void TriggerDeath()
    {
        MissionHolder.i.CheckValidMission (missionObject);
        audioSource.clip = deathSound;
        audioSource.Play();
        animator.SetTrigger("Die");
    }

    private void Die()
    {
        Destroy (gameObject);
    }

    public void AttackAnim()
    {
        animator.SetBool("Attack", true);
        audioSource.clip = attackSound;
        audioSource.Play();
    }

    public void WalkAnim()
    {
        animator.SetBool("Attack", false);
        audioSource.clip = walkSound;
        audioSource.Play();
    }
}
