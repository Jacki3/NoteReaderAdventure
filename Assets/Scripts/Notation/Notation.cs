using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro.Examples;
using UnityEngine;

public class Notation : MonoBehaviour
{
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

    public Color32 playedNoteColour;

    public GameObject notation;

    public ExplodableNotation explodableNotation;

    private SpriteRenderer parentRenderer;

    private bool isVisible;

    [HideInInspector]
    public List<int> notes = new List<int>();

    private List<SpriteRenderer> noteImages = new List<SpriteRenderer>();

    public bool arenaMode = false;

    public bool objectShow = false;

    private bool usingBass;

    private List<SpriteRenderer> playedNotes = new List<SpriteRenderer>();

    private string[]
        noteNames =
        {
            "C",
            "C#",
            "D",
            "D#",
            "E",
            "F",
            "F#",
            "G",
            "G#",
            "A",
            "A#",
            "B",
            "B#"
        };

    private void Awake()
    {
        parentRenderer = transform.parent.GetComponent<SpriteRenderer>();
        PlayerController.notationCircleActivated += ShowNotation;
        PlayerController.notationCircleDeactivated += HideNotation;
        MIDIController.NoteOn += ShowPlayedNote;
        MIDIController.NoteOff += DestroyPlayedNote;
    }

    void Start()
    {
        holder.SetActive(false);

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
            notes = NotesController.pattern.ToList();
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
            SpawnNotes(startingSpawnPosX, notes[i], usingBass, false);
            if (totalNotesToSpawn == 2)
                startingSpawnPosX += distanceBetweenNotesX + -startingSpawnPosX;
            else if (totalNotesToSpawn == 8 && i == 3)
                startingSpawnPosX += distanceBetweenNotesX * 2; //multiply to allow space between each bar
            else
                startingSpawnPosX += distanceBetweenNotesX;
        }

        if (arenaMode) ShowNotation();
    }

    void Update()
    {
        if (parentRenderer.isVisible)
        {
            isVisible = true;
        }
        else
        {
            isVisible = false;
        }
    }

    private SpriteRenderer
    SpawnNotes(float spawnX, int index, bool bass, bool isTemp)
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
            SpriteRenderer noteRenderer =
                transform.GetComponent<SpriteRenderer>();
            if (noteRenderer != null)
            {
                transform.GetComponent<SpriteRenderer>().sortingLayerName =
                    newNote.sortingLayerName;
                transform.GetComponent<SpriteRenderer>().sortingOrder =
                    newNote.sortingOrder;
            }

            transform.gameObject.SetActive(false);
        }

        int accidentalNote = index % 12;

        if (!isTemp)
        {
            switch (accidentalNote)
            {
                case 1:
                case 3:
                case 6:
                case 8:
                case 10:
                    if (Random.value < .5)
                    {
                        index -= 1;
                        newNote
                            .transform
                            .GetChild(7)
                            .gameObject
                            .SetActive(true);
                        newNote
                            .transform
                            .GetChild(7)
                            .gameObject
                            .GetComponent<TextMesh>()
                            .text = "♯";
                    }
                    else
                    {
                        index += 1;
                        newNote
                            .transform
                            .GetChild(7)
                            .gameObject
                            .SetActive(true);
                        newNote
                            .transform
                            .GetChild(7)
                            .gameObject
                            .GetComponent<TextMesh>()
                            .text = "♭";
                    }
                    break;
            }
            noteImages.Add (newNote);
        }
        else
        {
            newNote.color = playedNoteColour;
            newNote.transform.GetChild(6).GetComponent<SpriteRenderer>().color =
                playedNoteColour;
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

        return newNote;
    }

    public void ShowNotation()
    {
        if (this != null)
        {
            if (isVisible || arenaMode || objectShow)
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

                //do not add this - add the first child of the parent instead
                Notation firstNotation =
                    notation.transform.GetChild(0).GetComponent<Notation>();
                if (firstNotation != null)
                    NotationController.AddNotationToList_Static(firstNotation);
                notationAnimator.SetTrigger("Unhighlight");
            }
        }
    }

    public void HideNotation()
    {
        if (this != null)
            if (holder.activeSelf)
            {
                holder.SetActive(false);
                NotationController.RemoveNotationFromList_Static(this);
            }
    }

    public void PlayNote()
    {
        Color32 noteColour = Color.black;
        if (CoreGameElements.i.useColours)
            noteColour = CoreGameElements.i.noteColours[notes[0] % 12];

        string noteName = noteNames[notes[0] % 12];
        NotePopup
            .Create(noteImages[0].transform.position, noteName, noteColour);

        PlayerWeapon
            .ShootProjectileStatic(this.notation.transform.position,
            noteColour);

        EZCameraShake.CameraShaker.Instance.ShakeOnce(.45f, 1f, .5f, 1f);

        notationAnimator.SetTrigger("Correct");
        Destroy(noteImages[0].gameObject);
        notes.RemoveAt(0);
        noteImages.RemoveAt(0);

        INotation notationInterface = notation.GetComponent<INotation>();

        if (NotationFinished())
        {
            if (notation != null && notation.transform.childCount > 1)
            {
                Notation nextNotation =
                    notation.transform.GetChild(1).GetComponent<Notation>();
                if (nextNotation != null)
                    NotationController.AddNotationToList_Static(nextNotation);
            }

            // var explodedNotation = Instantiate(explodableNotation);
            // explodedNotation.transform.position = transform.position;
            PlayerController.notationCircleActivated -= ShowNotation;
            PlayerController.notationCircleDeactivated -= HideNotation;

            if (notationInterface != null)
            {
                notationInterface.NotationComplete();
            }

            Destroy (gameObject);
        }
        else if (notationInterface != null)
            notationInterface.PlayedCorrectNote();
    }

    public void IncorrecNote()
    {
        EZCameraShake.CameraShaker.Instance.ShakeOnce(.5f, 1f, .5f, 1f);
        SoundController.PlaySound(SoundController.Sound.IncorectNote);
        notationAnimator.SetTrigger("Incorrect");
    }

    public void ShowPlayedNote(int note, float t)
    {
        note -= MIDIController.startingMIDINumber;
        if (noteImages.Count > 0 && this != null)
            playedNotes
                .Add(SpawnNotes(noteImages[0].transform.localPosition.x,
                note,
                usingBass,
                true));
    }

    public void DestroyPlayedNote(int note)
    {
        if (playedNotes.Count > 0)
        {
            var SR = playedNotes[0];
            playedNotes.RemoveAt(0);
            if (SR != null) Destroy(SR.gameObject);
        }
    }

    public void HighlightNotation()
    {
        notationAnimator.SetTrigger("Highlight");
    }

    public void UnhighlightNotation()
    {
        notationAnimator.SetTrigger("Unhighlight");
    }

    public void AllIncorrect()
    {
        EZCameraShake.CameraShaker.Instance.ShakeOnce(.5f, 1f, .5f, 1f);
        SoundController.PlaySound(SoundController.Sound.IncorectNote);
        notationAnimator.SetTrigger("AllIncorrect");
    }

    public bool NotationFinished() => notes.Count <= 0;
}
