using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHealthCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            if (this.tag == "Enemy")
                HealthController.RemoveHealth(6, true);
            else
                HealthController.AddHealth(1);
    }
}
