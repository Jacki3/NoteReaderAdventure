using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : NotationEnemy
{
    public float speed;

    public Transform playerPos;

    public EnemySpawner enemySpawner;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
    }

    public override void Damage(IDamagable damagable)
    {
        enemySpawner.RemoveEnemy(this);
        base.Damage(damagable);
        Die();
    }

    public override void TriggerDeath()
    {
        enemySpawner.RemoveEnemy(this);
        Die();
    }
}
