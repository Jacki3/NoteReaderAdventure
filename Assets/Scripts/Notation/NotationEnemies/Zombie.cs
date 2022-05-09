using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : NotationEnemy
{
    public Transform target;

    public float speed;

    public float hitTime;

    public bool dirRight = true;

    private Vector3 originalPosition;

    private SpriteRenderer spriteRenderer;

    private bool dead;

    private bool isHurt;

    private float timeHurt = 0;

    private float originalSpeed;

    void Start()
    {
        originalSpeed = speed;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalPosition = transform.position;
    }

    private void Update()
    {
        if (dirRight)
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        else
            transform.Translate(-Vector2.right * speed * Time.deltaTime);

        if (transform.position.x >= target.position.x)
        {
            dirRight = false;
            spriteRenderer.flipX = true;
        }
        if (transform.position.x <= originalPosition.x)
        {
            dirRight = true;
            spriteRenderer.flipX = false;
        }
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

    public override void TriggerDeath()
    {
        base.TriggerDeath();
        speed = 0;
        timeHurt = 2;
        dead = true;
    }
}
