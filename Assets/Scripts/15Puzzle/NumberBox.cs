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

    public void Init(
        int i,
        int j,
        int _index,
        Sprite sprite,
        Action<int, int> _moveTile
    )
    {
        index = _index;
        UpdatePos (i, j);
        GetComponent<SpriteRenderer>().sprite = sprite;
        moveTile = _moveTile;
    }

    public void UpdatePos(int i, int j)
    {
        x = i;
        y = j;

        StartCoroutine(Slide());
    }

    public void PrintIndex()
    {
        print (index);
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

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && moveTile != null) moveTile(x, y);
    }
}
