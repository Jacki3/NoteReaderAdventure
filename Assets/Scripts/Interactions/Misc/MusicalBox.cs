using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicalBox : NotationItem
{
    public ItemSpawner.ItemType spawnableItem;

    public int totalItemsSpawnable = 1;

    public bool canSpawnHealth;

    public bool tutorialObject;

    private Explodable _explodable;

    private SpriteRenderer _spriteRenderer;

    private AudioSource _audioSource;

    void Start()
    {
        _explodable = GetComponent<Explodable>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }

    protected override void AllNotationsComplete()
    {
        base.AllNotationsComplete();
        DestroyObject();
    }

    public void DestroyObject()
    {
        //if health is below certain value then spawn lots of health OR if it is slightly low then spawn some (random)
        if (canSpawnHealth && HealthController.CriticalHealth())
        {
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
        if (tutorialObject)
        {
            if (GameStateController.state == GameStateController.States.Tutorial
            )
            {
                TutorialManager
                    .CheckTutorialStatic(Tutorial.TutorialValidation.ReadMode);
            }
        }

        ExplosionForce ef = GameObject.FindObjectOfType<ExplosionForce>();
        ef.force = 20;
        ef.radius = 5;
        _explodable.explode (_spriteRenderer);
        ef.doExplosion(transform.position);
    }
}
