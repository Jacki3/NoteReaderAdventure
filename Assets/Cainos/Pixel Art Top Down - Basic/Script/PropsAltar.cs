using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//when something get into the alta, make the runes glow
namespace Cainos.PixelArtTopDown_Basic
{
    public class PropsAltar : MonoBehaviour
    {
        public EnemySpawner enemySpawner;

        public AudioSource musicSource;

        public AudioClip arenaMusic;

        public Animator screenFlash;

        public PlayerController player;

        public Transform arenaSpawn;

        public List<SpriteRenderer> runes;

        public float lerpSpeed;

        public static bool arenaMode;

        private Color curColor;

        private Color targetColor;

        private void OnTriggerEnter2D(Collider2D other)
        {
            targetColor = new Color(1, 1, 1, 1);
            if (other.tag == "Stone")
            {
                enemySpawner.enabled = true;
                musicSource.clip = arenaMusic;
                musicSource.Play();
                screenFlash.SetTrigger("StartGame");
                player.transform.position = arenaSpawn.position;
                player.SetReadingMode();
                arenaMode = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            targetColor = new Color(1, 1, 1, 0);
        }

        private void Update()
        {
            curColor =
                Color.Lerp(curColor, targetColor, lerpSpeed * Time.deltaTime);

            foreach (var r in runes)
            {
                r.color = curColor;
            }
        }
    }
}
