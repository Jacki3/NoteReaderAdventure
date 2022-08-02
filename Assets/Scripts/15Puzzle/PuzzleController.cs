using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.Mathematics;

using Random = UnityEngine.Random;

public class PuzzleController : MonoBehaviour
{
    public NumberBox boxPrefab;

    public StartMenu startMenu;

    public int[] notes = { 0, 2, 4, 5, 7, 9, 11, -1 };

    public NumberBox[,] boxes = new NumberBox[4, 2];

    public Sprite[] numberSprites;

    private Vector2 lastMove;

    private int[] correctOrder = { 5, 1, 6, 2, 7, 3, 8, 4 };

    public List<NumberBox> allBoxes = new List<NumberBox>();

    private Canvas puzzleCanvas;

    void Start()
    {
        puzzleCanvas = GetComponentInChildren<Canvas>();
        puzzleCanvas.enabled = false;
    }

    public void GeneratePuzzle()
    {
        puzzleCanvas.enabled = true;
        if (allBoxes.Count <= 0)
            Init();
        else
            foreach (NumberBox box in boxes)
            {
                box.gameObject.SetActive(true);
            }
        for (int i = 0; i < correctOrder.Length; i++) ShuffleBoard();
    }

    private void Init()
    {
        int index = 0;
        for (int y = 1; y >= 0; y--)
        {
            for (int x = 0; x < 4; x++)
            {
                NumberBox box =
                    Instantiate(boxPrefab, Vector2.zero, Quaternion.identity);
                allBoxes.Add (box);
                int spriteIndex = notes[index];
                if (spriteIndex == -1) spriteIndex = numberSprites.Length - 1;
                box
                    .Init(x,
                    y,
                    index + 1,
                    numberSprites[spriteIndex],
                    ClickToMoveTile,
                    spriteIndex);
                boxes[x, y] = box;
                index++;
            }
        }
    }

    void Update()
    {
        print(GameStateController.state);
    }

    private void ClickToMoveTile(int x, int y)
    {
        int getX = GetX(x, y);
        int getY = GetY(x, y);

        SwapTile (x, y, getX, getY);

        //create a list of the numbers in order of each box
        List<int> boxNumbers = new List<int>();
        foreach (NumberBox box in boxes)
        {
            boxNumbers.Add(box.index);
        }

        //check if the list above is exactly equal to the correct oder of notes
        bool isEqual = Enumerable.SequenceEqual(correctOrder, boxNumbers);

        //if it is, you win
        if (isEqual)
        {
            FinishPuzzle();
        }
    }

    public void FinishPuzzle()
    {
        puzzleCanvas.enabled = false;
        foreach (NumberBox box in boxes)
        {
            box.gameObject.SetActive(false);
        }
        LevelController.i.levelLoader.LoadLevel(-1);
        startMenu.StartGame();
        gameObject.SetActive(false);
    }

    private void SwapTile(int x, int y, int getX, int getY)
    {
        var from = boxes[x, y];
        int actualY = y + getY;
        if (actualY == 2 || actualY == 3) actualY = Random.Range(0, 2);

        var target = boxes[x + getX, actualY];

        //swap the tiles
        boxes[x, y] = target;
        boxes[x + getX, actualY] = from;

        //update the two tiles pos
        from.UpdatePos(x + getX, actualY);
        target.UpdatePos (x, y);
    }

    private int GetX(int x, int y)
    {
        //if right is empty
        if (x < 3 && boxes[x + 1, y].IsEmpty(correctOrder.Length)) return 1;

        //if left is empty
        if (x > 0 && boxes[x - 1, y].IsEmpty(correctOrder.Length)) return -1;

        return 0;
    }

    private int GetY(int x, int y)
    {
        //if above is empty
        if (y < 1 && boxes[x, y + 1].IsEmpty(correctOrder.Length)) return 1;

        //if below is empty
        if (y > 0 && boxes[x, y - 1].IsEmpty(correctOrder.Length)) return -1;

        return 0;
    }

    public void NewShuffle()
    {
        for (int i = 0; i < 99; i++) ShuffleBoard();
    }

    public void ShuffleBoard()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                if (boxes[i, j].IsEmpty(correctOrder.Length))
                {
                    Vector2 pos = GetValidMove(i, j);
                    SwapTile(i, j, (int) pos.x, (int) pos.y);
                }
            }
        }
    }

    private Vector2 GetValidMove(int x, int y)
    {
        Vector2 pos = new Vector2();
        do
        {
            int rand = Random.Range(0, 4);
            if (rand == 0)
                pos = Vector2.up;
            else if (rand == 1)
                pos = Vector2.down;
            else if (rand == 2)
                pos = Vector2.left;
            else
                pos = Vector2.right;
        }
        while (!(
            IsValidRange(x + (int) pos.x) && IsValidRange(y + (int) pos.y)
            ) ||
            IsRepeatMove(pos)
        );
        lastMove = pos;
        return pos;
    }

    private bool IsValidRange(int rand)
    {
        return rand >= 0 && rand <= 3;
    }

    private bool IsRepeatMove(Vector2 pos)
    {
        return pos * -1 == lastMove;
    }
}
