using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour
{
    public ItemSpawner.ItemType spawnableItem;

    public SoundController.Sound soundType;

    public int totalItemsSpawnable = 1;

    public bool canSpawnHealth;

    private Explodable _explodable;

    private SpriteRenderer _spriteRenderer;

    private AudioSource _audioSource;

    void Start()
    {
        _explodable = GetComponent<Explodable>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //something about interfaces...?
        if (other.tag == "SmashCircle")
        {
            DestroyObject();
        }
    }

    protected virtual void DestroyObject()
    {
        //if health is below certain value then spawn lots of health OR if it is slightly low then spawn some (random)
        if (canSpawnHealth && HealthController.CriticalHealth())
        {
            //spawn different health sizes depending on how low it is?
            ItemSpawner
                .SpawnItem(ItemSpawner.ItemType.LargeHealthPotion,
                transform.position,
                1,
                _spriteRenderer);
        }
        else if (canSpawnHealth && HealthController.LowHealth())
        {
            ItemSpawner
                .SpawnItem(ItemSpawner.ItemType.HealthPotion,
                transform.position,
                1,
                _spriteRenderer);
        }
        else if (canSpawnHealth && HealthController.NotMaxHealth())
        {
            ItemSpawner
                .SpawnItem(ItemSpawner.ItemType.SmallHealthPotion,
                transform.position,
                1,
                _spriteRenderer);
        }
        else
        {
            ItemSpawner
                .SpawnItem(spawnableItem,
                transform.position,
                totalItemsSpawnable,
                _spriteRenderer);
        }
        SoundController.PlaySound (soundType);

        _explodable.explode (_spriteRenderer);
        ExplosionForce ef = GameObject.FindObjectOfType<ExplosionForce>();
        ef.doExplosion(transform.position);
    }
}
