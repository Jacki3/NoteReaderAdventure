using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : NotationEnemy
{
    public Transform target;

    public float speed;

    private Vector3 originalPosition;

    private SpriteRenderer spriteRenderer;

    private bool dead;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalPosition = transform.position;
    }

    private void Update()
    {
        if (!dead)
            transform.position =
                Vector2
                    .Lerp(originalPosition,
                    target.position,
                    Mathf.PingPong(Time.time / speed, 1));

        if (Vector2.Distance(transform.position, target.position) < .1f)
            spriteRenderer.flipX = true;
        if (Vector2.Distance(transform.position, originalPosition) < .1f)
            spriteRenderer.flipX = false;
    }

    public override void TriggerDeath()
    {
        base.TriggerDeath();
        dead = true;
    }
}
