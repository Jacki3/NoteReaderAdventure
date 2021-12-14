using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollision : MonoBehaviour
{
    private Zombie zombie;

    private void Awake()
    {
        zombie = transform.root.gameObject.GetComponent<Zombie>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            zombie.AttackAnim();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            zombie.WalkAnim();
        }
    }
}
