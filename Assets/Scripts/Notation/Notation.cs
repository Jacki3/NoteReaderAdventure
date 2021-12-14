using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notation : MonoBehaviour
{
    public Explodable explodableNotation;

    public GameObject holder;

    public Animator notationAnimator;

    public float distanceBetweenNotesX;

    public float startingSpawnPosX;

    public SpriteRenderer noteImage;

    public Transform[] noteSpawnsY;

    public int totalNotesToSpawn = 4;

    public bool usePattern;

    private SpriteRenderer parentRenderer;

    private bool isVisible;

    [HideInInspector]
    public List<int> notes = new List<int>();

    private List<SpriteRenderer> noteImages = new List<SpriteRenderer>();

    private List<int[]> possiblePatterns = new List<int[]>();

    private void Awake()
    {
        parentRenderer = transform.parent.GetComponent<SpriteRenderer>();
        PlayerController.notationCircleActivated += ShowNotation;
        PlayerController.notationCircleDeactivated += HideNotation;
    }

    void Start()
    {
        holder.SetActive(false);

        if (usePattern && NotesController.CanUsePattern(totalNotesToSpawn))
        {
            int patternIndex = 0;
            for (int i = 0; i < totalNotesToSpawn; i++)
            {
                notes.Add(NotesController.pattern[patternIndex]);
                patternIndex++;
            }
        }
        else
        {
            for (int i = 0; i < totalNotesToSpawn; i++)
            {
                int randNote = NotesController.GetRandomNote();
                notes.Add (randNote);
            }
        }

        for (int i = 0; i < totalNotesToSpawn; i++)
        {
            SpawnNotes(startingSpawnPosX, notes[i]);
            if (totalNotesToSpawn == 2)
                startingSpawnPosX += distanceBetweenNotesX + -startingSpawnPosX;
            else
                startingSpawnPosX += distanceBetweenNotesX;
        }
    }

    void Update()
    {
        if (parentRenderer.isVisible)
            isVisible = true;
        else
            isVisible = false;
    }

    private void SpawnNotes(float spawnX, int index)
    {
        SpriteRenderer newNote = Instantiate(noteImage, holder.transform);
        SpriteRenderer holderChild =
            holder.transform.GetChild(1).GetComponent<SpriteRenderer>();
        newNote.sortingLayerName = holderChild.sortingLayerName;
        newNote.sortingOrder = holderChild.sortingOrder + 1;

        newNote.transform.localPosition =
            new Vector2(spawnX, noteSpawnsY[index].localPosition.y);
        if (CoreGameElements.i.useColours)
            newNote.color = CoreGameElements.i.noteColours[index % 12];

        switch (index)
        {
            case 0:
            case 21:
                newNote.transform.GetChild(0).gameObject.SetActive(true);
                var ledgerSpriteRenderer =
                    newNote
                        .transform
                        .GetChild(0)
                        .GetComponent<SpriteRenderer>();
                ledgerSpriteRenderer.sortingLayerName =
                    holderChild.sortingLayerName;
                ledgerSpriteRenderer.sortingOrder = holderChild.sortingOrder;
                break;
            case 23:
                newNote.transform.GetChild(1).gameObject.SetActive(true);
                var ledgerSpriteRendererLow =
                    newNote
                        .transform
                        .GetChild(0)
                        .GetComponent<SpriteRenderer>();
                ledgerSpriteRendererLow.sortingLayerName =
                    holderChild.sortingLayerName;
                ledgerSpriteRendererLow.sortingOrder = holderChild.sortingOrder;
                break;
        }
        noteImages.Add (newNote);
    }

    private void ShowNotation()
    {
        if (isVisible)
        {
            holder.SetActive(true);
            NotationController.AddNotationToList_Static(this);
        }
    }

    private void HideNotation()
    {
        if (holder.activeSelf)
        {
            holder.SetActive(false);
            NotationController.RemoveNotationFromList_Static(this);
        }
    }

    public void PlayNote()
    {
        if (AudioController.canPlay)
            ScoreController
                .AddRhythmScore_Static(CoreGameElements.i.scoreForRhythm);

        notationAnimator.SetTrigger("Correct");
        Destroy(noteImages[0].gameObject);
        notes.RemoveAt(0);
        noteImages.RemoveAt(0);

        if (NotationFinished())
        {
            PlayerController.notationCircleActivated -= ShowNotation;
            PlayerController.notationCircleDeactivated -= HideNotation;
            INotation notation = transform.root.GetComponent<INotation>();
            if (notation != null) notation.NotationComplete();

            // ExplosionForce ef = GameObject.FindObjectOfType<ExplosionForce>();
            // ef.force = 350;
            // ef.radius = 10;
            // explodableNotation
            //     .explode(explodableNotation.GetComponent<SpriteRenderer>());
            // ef.doExplosion(transform.position);
            Destroy (gameObject);
        }
    }

    public void HighlightNotation()
    {
        notationAnimator.SetTrigger("Highlight");
    }

    public bool NotationFinished() => notes.Count <= 0;
}
