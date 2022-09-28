using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : NotationEnemy
{
    public float speed;

    public float hitTime;

    public Transform playerPos;

    public EnemySpawner enemySpawner;

    private SpriteRenderer spriteRenderer;

    private float timeHurt = 0;

    private float originalSpeed;

    protected virtual void Awake()
    {
        originalSpeed = speed;
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource.Play();
    }

    public virtual void Update()
    {
        transform.position =
            Vector2
                .MoveTowards(transform.position,
                playerPos.position,
                Time.deltaTime * speed);

        //do fancy AI stuff
        //flip my sprite depending on player pos to me?
        if (transform.position.x < playerPos.transform.position.x)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;

        if (timeHurt <= 0)
            speed = originalSpeed;
        else
            timeHurt -= Time.deltaTime;
    }

    public override void TakeDamage()
    {
        base.TakeDamage();
        speed = 0;
        timeHurt = hitTime;
    }

    public override void Damage(IDamagable damagable)
    {
        // enemySpawner.RemoveEnemy(this);
        base.Damage(damagable);
        Die();
    }

    public override void TriggerDeath()
    {
        if (deathSound != SoundController.Sound.None)
            SoundController.PlaySound(deathSound);

        // enemySpawner.RemoveEnemy(this);
        Die();
    }
}
