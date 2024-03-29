﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmEnemy : MovingObject, INotation
{
    public int scoreToAdd;

    public int XPToAdd;

    public int playerDmg;

    public int notationsToComplete;

    public int minDelay;

    public int maxDelay;

    public Mission.Object missionObject;

    public SoundController.Sound completeSound;

    private int beatDelay;

    private int notationsCompleted = 0;

    protected Animator animator;

    private Transform target;

    private float secPerBeat;

    private Explodable _explodable;

    private SpriteRenderer _spriteRenderer;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        _explodable = GetComponent<Explodable>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        target = GameObject.FindGameObjectWithTag("Player").transform; //this might be untrue for all enemies (just bear in mind)
        StartCoroutine(MoveToBeat());

        beatDelay = Random.Range(minDelay, maxDelay);
        if (beatDelay % 2 != 0) beatDelay++;
    }

    void Update()
    {
        secPerBeat = AudioController.secPerBeat;
    }

    private IEnumerator MoveToBeat()
    {
        while (true)
        {
            MoveEnemy();
            yield return new WaitForSeconds(secPerBeat * beatDelay);
            MoveEnemy();
            yield return new WaitForSeconds(secPerBeat * beatDelay);
        }
    }

    protected virtual void MoveEnemy()
    {
        //some enemies might have their own movement and when player is close enough then they will follow this logic
        int xDir = 0;
        int yDir = 0;

        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
        {
            yDir = target.position.y > transform.position.y ? 1 : -1;
        }
        else
        {
            xDir = target.position.x > transform.position.x ? 1 : -1;
        }

        AttemptMove<RigidPlayerController> (xDir, yDir);
    }

    protected override void OnCantMove<T>(T Component)
    {
    }

    public void NotationComplete()
    {
        print("notation complete");

        LevelController.i.levelLoader.boardController.RemoveNotationFromList (
            transform
        );
        notationsCompleted++;
        if (notationsCompleted >= notationsToComplete)
        {
            AllNotationsComplete();
        }
        else
        {
            TakeDamage();
        }
    }

    protected virtual void AllNotationsComplete()
    {
        if (XPToAdd > 0) ExperienceController.AddXP(XPToAdd);
        if (scoreToAdd > 0) ScoreController.AddScore_Static(scoreToAdd);
        if (missionObject != Mission.Object.None)
            MissionHolder.i.CheckValidMission(missionObject);
        if (completeSound != SoundController.Sound.None)
            SoundController.PlaySound(completeSound);

        var notationFloating =
            transform.GetComponentInChildren<ParticleSystem>();
        if (notationFloating != null)
            notationFloating.gameObject.SetActive(false);

        ExplosionForce ef = GameObject.FindObjectOfType<ExplosionForce>();
        ef.force = 20;
        ef.radius = 5;
        _explodable.explode (_spriteRenderer);
        ef.doExplosion(transform.position);
    }

    protected virtual void TakeDamage()
    {
    }

    public void PlayedCorrectNote()
    {
        CorrectNote();
    }

    protected virtual void CorrectNote()
    {
    }

    void OnBecameInvisible()
    {
        foreach (Transform child in transform)
        {
            Notation notation = child.GetComponent<Notation>();
            if (notation != null)
            {
                notation.objectShow = false;
                notation.HideNotation();
            }
        }
    }

    void OnBecameVisible()
    {
        if (PlayerController.readingMode)
        {
            foreach (Transform child in transform)
            {
                Notation notation = child.GetComponent<Notation>();
                if (notation != null)
                {
                    notation.objectShow = true;
                    notation.ShowNotation();
                }
            }
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public int GetObjectScore()
    {
        return scoreToAdd;
    }
}
