using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteProjectile : MonoBehaviour
{
    public float speed = 1;

    public Vector3 target;

    public Color32 projectileColour;

    public SpriteRenderer spriteRenderer;

    void Update()
    {
        spriteRenderer.color = projectileColour;
        float step = speed * Time.deltaTime;
        transform.position =
            Vector3.MoveTowards(transform.position, target, step);

        if (Vector3.Distance(transform.position, target) < 0.001f)
        {
            Destroy (gameObject);
        }
    }
}
