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

        public ReadingCircle readingCircle;

        public GameObject missionPlaceholder;

        public AudioHelm.AudioHelmClock helmClock;

        public static bool arenaMode;

        private Color curColor;

        private Color targetColor;

        private void OnTriggerEnter2D(Collider2D other)
        {
            targetColor = new Color(1, 1, 1, 1);
            if (other.tag == "Stone")
            {
                Invoke("StartArenaMode", 1.5f);
            }
        }

        private void StartArenaMode()
        {
            enemySpawner.enabled = true;
            missionPlaceholder.SetActive(false);
            helmClock.bpm = 131;
            musicSource.clip = arenaMusic;
            musicSource.pitch = 1;
            musicSource.Play();
            player.transform.position = arenaSpawn.position;
            readingCircle.cameraZoomSize = 4.5f;
            player.SetReadingMode();
            screenFlash.SetTrigger("StartGame");

            arenaMode = true;
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
