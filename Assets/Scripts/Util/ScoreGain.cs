using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreGain : MonoBehaviour
{
    public CoreUIElements.Text scoreText;

    public float _lifeTime = 3;

    public Transform spawnPoint;

    private static ScoreGain instance;

    private float spawnTime;

    private float lifeTime = 0;

    private Vector3 ascenionRate = Vector3.up;

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        var progress = (Time.time - spawnTime) / lifeTime;

        if (progress < 1)
        {
            transform.position += ascenionRate * Time.deltaTime * 1;

            var textColour = scoreText.text.color;
            textColour.a = 1 - progress;
            scoreText.text.color = textColour;
        }
    }

    public void SetScoreGain(string text)
    {
        spawnTime = Time.time;
        lifeTime = _lifeTime;

        gameObject.SetActive(true);
        transform.localPosition = spawnPoint.localPosition;
        scoreText.text.text = text;
    }

    public static void SetScoreGain_Static(string text)
    {
        instance.SetScoreGain (text);
    }
}
