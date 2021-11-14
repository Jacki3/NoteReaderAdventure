using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour
{
    public ItemSpawner.ItemType spawnableItem = ItemSpawner.ItemType.Coin;

    public int totalItemsSpawnable = 1;

    public int XPToAdd = 10;

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
        if (other.tag == "SmashCircle")
        {
            //if health is below certain value (create function for this in health script e.g. bool critialLow() then spawn hearts which can CHOICE: refill some or total health?)
            ExperienceController.AddXP (XPToAdd);
            ItemSpawner
                .SpawnItem(spawnableItem,
                transform.position,
                totalItemsSpawnable);
            SoundController.PlaySound(SoundController.Sound.PotBreak);

            _explodable.explode (_spriteRenderer);
            ExplosionForce ef = GameObject.FindObjectOfType<ExplosionForce>();
            ef.doExplosion(transform.position);
        }
    }
}
