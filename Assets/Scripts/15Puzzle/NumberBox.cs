using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberBox : MonoBehaviour
{
    public int index = 0;

    public float slideDuration;

    private int x = 0;

    private int y = 0;

    private Action<int, int> moveTile = null;

    private int note;

    private KeyCode[]
        keyCodes =
        {
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6,
            KeyCode.Alpha7,
            KeyCode.Alpha8,
            KeyCode.Alpha9
        };

    void Awake()
    {
        MIDIController.NoteOn += CheckMoveTile;
        MIDIController.NoteOff += NoteOff;
    }

    void Update()
    {
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                int numberPressed = i + 1;
                CheckMoveTile(numberPressed, 0);
            }
        }
    }

    public void Init(
        int i,
        int j,
        int _index,
        Sprite sprite,
        Action<int, int> _moveTile,
        int _note
    )
    {
        index = _index;
        UpdatePos (i, j);
        GetComponent<SpriteRenderer>().sprite = sprite;
        moveTile = _moveTile;
        note = _note;
    }

    public void UpdatePos(int i, int j)
    {
        x = i;
        y = j;

        StartCoroutine(Slide());
    }

    private IEnumerator Slide()
    {
        float elapsedTime = 0;
        Vector2 start = transform.localPosition;
        Vector2 end = new Vector2(x, y);

        while (elapsedTime < slideDuration)
        {
            transform.localPosition =
                Vector2.Lerp(start, end, (elapsedTime / slideDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = end;
    }

    public bool IsEmpty(int maxTiles)
    {
        return (index == maxTiles);
    }

    private void CheckMoveTile(int number, float t)
    {
        if (GameStateController.state == GameStateController.States.Puzzle)
            if (number == index || number % 12 == note)
                if (moveTile != null) moveTile(x, y);
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && moveTile != null) moveTile(x, y);
    }

    private void NoteOff(int note)
    {
        //Ensures MIDI controller has handling for note off events
    }
}
