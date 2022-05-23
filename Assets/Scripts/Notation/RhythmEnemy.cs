using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmEnemy : MovingObject, INotation
{
    public int playerDmg;

    public int beatDelay = 1;

    public int notationsToComplete;

    private int notationsCompleted = 0;

    private Animator animator;

    private Transform target;

    private float secPerBeat;

    protected override void Start()
    {
        base.Start();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(MoveToBeat());

        beatDelay = Random.Range(2, 8);
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

    private void MoveEnemy()
    {
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
        RigidPlayerController hitPlayer = Component as RigidPlayerController;
        hitPlayer.LoseHealth (playerDmg);
    }

    public void NotationComplete()
    {
        notationsCompleted++;
        if (notationsCompleted >= notationsToComplete)
        {
            print("destroy");
            Destroy (gameObject);
        }
        else
        {
            //take damage - play sound, animate etc.
        }
    }

    public void PlayedCorrectNote()
    {
        print("dmg");

        //take damage - play sound, animate etc.
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
}
