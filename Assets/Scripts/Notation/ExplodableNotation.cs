using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodableNotation : MonoBehaviour
{
    private Explodable explodableNotation;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        explodableNotation = GetComponent<Explodable>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        explodableNotation.explode (spriteRenderer);
        ExplosionForce ef = GameObject.FindObjectOfType<ExplosionForce>();
        ef.force = 15;
        ef.radius = 1;
        ef.doExplosion(transform.position);
    }
}
