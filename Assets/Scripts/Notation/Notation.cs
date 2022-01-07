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

    public SpriteRenderer clefRenderer;

    public Sprite bassSprite;

    public Sprite trebleSprite;

    public Transform[] noteSpawnsY;

    public int totalNotesToSpawn = 4;

    public bool usePattern;

    private SpriteRenderer parentRenderer;

    private bool isVisible;

    [HideInInspector]
    public List<int> notes = new List<int>();

    private List<SpriteRenderer> noteImages = new List<SpriteRenderer>();

    public bool arenaMode = false;

    private void Awake()
    {
        parentRenderer = transform.parent.GetComponent<SpriteRenderer>();
        PlayerController.notationCircleActivated += ShowNotation;
        PlayerController.notationCircleDeactivated += HideNotation;
    }

    void Start()
    {
        holder.SetActive(false);

        bool usingBass;
        if (
            CoreGameElements.i.useBassNotes &&
            Random.value > CoreGameElements.i.chanceOfBassNotes
        )
            usingBass = true;
        else
            usingBass = false;

        if (
            usePattern &&
            NotesController.CanUsePattern(totalNotesToSpawn, usingBass)
        )
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
                int randNote = NotesController.GetRandomNote(usingBass);
                notes.Add (randNote);
            }
        }

        for (int i = 0; i < totalNotesToSpawn; i++)
        {
            SpawnNotes(startingSpawnPosX, notes[i], usingBass);
            if (totalNotesToSpawn == 2)
                startingSpawnPosX += distanceBetweenNotesX + -startingSpawnPosX;
            else
                startingSpawnPosX += distanceBetweenNotesX;
        }

        if (arenaMode) ShowNotation();
    }

    void Update()
    {
        if (parentRenderer.isVisible)
            isVisible = true;
        else
            isVisible = false;
    }

    private void SpawnNotes(float spawnX, int index, bool bass)
    {
        if (bass)
            clefRenderer.sprite = bassSprite;
        else
            clefRenderer.sprite = trebleSprite;

        SpriteRenderer newNote = Instantiate(noteImage, holder.transform);
        SpriteRenderer holderChild =
            holder.transform.GetChild(1).GetComponent<SpriteRenderer>();
        newNote.sortingLayerName = holderChild.sortingLayerName;
        newNote.sortingOrder = holderChild.sortingOrder + 2;

        newNote.enabled = true;

        foreach (Transform transform in newNote.transform)
        {
            transform.GetComponent<SpriteRenderer>().sortingLayerName =
                newNote.sortingLayerName;
            transform.GetComponent<SpriteRenderer>().sortingOrder =
                newNote.sortingOrder;

            transform.gameObject.SetActive(false);
        }

        newNote.transform.localPosition =
            new Vector2(spawnX, noteSpawnsY[index].localPosition.y);

        switch (index)
        {
            case 0:
                newNote.transform.GetChild(0).gameObject.SetActive(true);
                newNote.transform.GetChild(3).gameObject.SetActive(true);
                break;
            case 2:
                newNote.transform.GetChild(2).gameObject.SetActive(true);
                break;
            case 4:
            case 24:
                newNote.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case 45:
                newNote.flipX = true;
                newNote.flipY = true;
                newNote.transform.GetChild(4).gameObject.SetActive(true);
                break;
            case 47:
                newNote.flipX = true;
                newNote.flipY = true;
                newNote.transform.GetChild(5).gameObject.SetActive(true);
                break;
            case 14:
            case 16:
            case 17:
            case 19:
            case 21:
            case 23:
            case 36:
            case 38:
            case 40:
            case 41:
            case 43:
                newNote.transform.GetChild(6).gameObject.SetActive(true);
                newNote.enabled = false;
                break;
        }
        noteImages.Add (newNote);
    }

    private void ShowNotation()
    {
        if (isVisible || arenaMode)
        {
            int index = 0;
            foreach (SpriteRenderer note in noteImages)
            {
                if (CoreGameElements.i.useColours)
                {
                    note.color =
                        CoreGameElements.i.noteColours[notes[index] % 12];
                    note
                        .transform
                        .GetChild(6)
                        .GetComponent<SpriteRenderer>()
                        .color =
                        CoreGameElements.i.noteColours[notes[index] % 12];
                    index++;
                }
                else
                {
                    note.color = Color.black;
                    note
                        .transform
                        .GetChild(6)
                        .GetComponent<SpriteRenderer>()
                        .color = Color.black;
                }
            }
            holder.SetActive(true);
            NotationController.AddNotationToList_Static(this);
            notationAnimator.SetTrigger("Unhighlight");
        }
    }

    private void HideNotation()
    {
        if (holder.activeSelf)
        {
            UnhighlightNotation();
            NotationController.RemoveNotationFromList_Static(this);
        }
    }

    public void PlayNote()
    {
        EZCameraShake.CameraShaker.Instance.ShakeOnce(.45f, 1f, .5f, 1f);

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
            Destroy (gameObject);
        }
    }

    public void IncorrecNote()
    {
        notationAnimator.SetTrigger("Incorrect");
        EZCameraShake.CameraShaker.Instance.ShakeOnce(.5f, 1f, .5f, 1f);
        SoundController.PlaySound(SoundController.Sound.IncorectNote);
    }

    public void HighlightNotation()
    {
        notationAnimator.SetTrigger("Highlight");
    }

    public void UnhighlightNotation()
    {
        holder.SetActive(false);
    }

    public bool NotationFinished() => notes.Count <= 0;
}
