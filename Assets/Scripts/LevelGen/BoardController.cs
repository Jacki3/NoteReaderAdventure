using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

//Layouts random level pattern based on current level number
public class BoardController : MonoBehaviour
{
    [System.Serializable]
    public class Count
    {
        public int minimum;

        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 10;

    public int rows = 10;

    public Count propCount = new Count(5, 9);

    public Count notationCount = new Count(5, 9);

    public GameObject[] floorTiles;

    public GameObject[] outerWallTiles;

    public GameObject[] propTiles;

    public GameObject[] notationTiles;

    public GameObject exit;

    private Transform boardHolder;

    private List<Vector2> gridPositions = new List<Vector2>();

    void Start()
    {
        SetupScene(3);
    }

    void InitialiseList()
    {
        gridPositions.Clear();

        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows; y++)
            {
                GameObject toInstantiate =
                    floorTiles[Random.Range(0, floorTiles.Length)];
                if (x == -1 || x == columns || y == -1 || y == rows)
                    toInstantiate =
                        outerWallTiles[Random.Range(0, outerWallTiles.Length)];

                GameObject instance =
                    Instantiate(toInstantiate,
                    new Vector3(x, y, 0f),
                    Quaternion.identity) as
                    GameObject;

                instance.transform.SetParent (boardHolder);
            }
        }
    }

    Vector2 RandomPos()
    {
        int randIndex = Random.Range(0, gridPositions.Count);
        Vector2 randPos = gridPositions[randIndex];
        gridPositions.RemoveAt (randIndex);

        return randPos;
    }

    void LayoutObjectAtRand(GameObject[] tileArray, int min, int max)
    {
        int objectCount = Random.Range(min, max + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector2 randomPos = RandomPos();
            GameObject tileChoice =
                tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPos, Quaternion.identity);
        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitialiseList();
        // LayoutObjectAtRand(propTiles, propCount.minimum, propCount.maximum);
        // LayoutObjectAtRand(notationTiles,
        // notationCount.minimum,
        // notationCount.maximum);

        // int enemyCount = (int) Mathf.Log(level, 2f);

        // Instantiate(exit,
        // new Vector3(columns - 1, rows - 1, 0f),
        // Quaternion.identity);
    }
}
