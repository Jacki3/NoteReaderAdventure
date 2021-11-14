using System.Collections;
using UnityEngine;

[RequireComponent(typeof (Explodable))]
public class ExplodeOnClick : MonoBehaviour
{
    private Explodable _explodable;

    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _explodable = GetComponent<Explodable>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown()
    {
        _explodable.explode (_spriteRenderer);
        ExplosionForce ef = GameObject.FindObjectOfType<ExplosionForce>();
        ef.doExplosion(transform.position);
    }
}
