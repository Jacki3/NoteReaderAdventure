using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

using Random = UnityEngine.Random;

//Layouts random level pattern and enemies/notation/pots based on level number
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

    public bool firstTimeSetup = true;

    [Header("Dimensions")]
    public int columsMin;

    public int columnsMax = 10;

    public int rowsMin;

    public int rowsMax = 10;

    public int maxSize = 24;

    public bool randomSize = false;

    [Header("Counts")]
    public Count propCount = new Count(5, 9);

    public Count smashableCount = new Count(5, 9);

    public Count notationCount = new Count(5, 9);

    public Count enemyCount = new Count(1, 1);

    public bool spawnNotation;

    public bool spawnEnemies;

    [Header("Tiles")]
    public float leftCornerPos = -.25f;

    public GameObject leftCornerBottom;

    public GameObject leftCornerTop;

    public GameObject rightCornerBottom;

    public GameObject rightCornerTop;

    public GameObject danceFloorLeft;

    public GameObject danceFloorRight;

    public GameObject[] floorTiles;

    public GameObject[] outerLeftWallTiles;

    public GameObject[] outerRightWallTiles;

    public GameObject[] outerTopWallTiles;

    public GameObject[] outerBottomWallTiles;

    public GameObject[] propTiles;

    public GameObject[] notationTiles;

    public GameObject[] enemyTiles;

    public GameObject[] smashableTiles;

    public GameObject exit;

    public List<INotation> notations = new List<INotation>();

    private Transform boardHolder;

    private Transform tiles;

    private Transform objects;

    private Transform danceFloor;

    private List<Vector2> gridPositions = new List<Vector2>();

    private float rightCornerPos;

    private int columns;

    private int rows;

    private RhythmFlash flashAnim;

    private bool danceFloorHidden = false;

    void Awake()
    {
        flashAnim = GetComponent<RhythmFlash>();
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
        tiles = new GameObject("Tiles").transform;
        tiles.SetParent (boardHolder);
        objects = new GameObject("Objects").transform;
        objects.SetParent (boardHolder);
        danceFloor = new GameObject("DanceFloor").transform;
        danceFloor.SetParent (boardHolder);

        GameObject cornerLeft = leftCornerBottom;
        GameObject cornerRight = rightCornerBottom;

        float leftCornerY = -1;
        float rightCornerY = -1;

        rightCornerPos = columns - (1 + leftCornerPos);

        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate =
                    floorTiles[Random.Range(0, floorTiles.Length)];

                //Spawning outer walls
                if (x == -1)
                    toInstantiate =
                        outerLeftWallTiles[Random
                            .Range(0, outerLeftWallTiles.Length)];
                else if (x == columns)
                    toInstantiate =
                        outerRightWallTiles[Random
                            .Range(0, outerRightWallTiles.Length)];
                else if (y == -1)
                    toInstantiate =
                        outerBottomWallTiles[Random
                            .Range(0, outerBottomWallTiles.Length)];
                else if (y == rows)
                    toInstantiate =
                        outerTopWallTiles[Random
                            .Range(0, outerTopWallTiles.Length)];

                //Handling Corners
                if (x == -1 && y == -1 || x == -1 && y == rows)
                {
                    toInstantiate = null;
                    SpawnTile(new Vector3(leftCornerPos, leftCornerY),
                    cornerLeft);
                    leftCornerY += (rows - leftCornerPos);
                    cornerLeft = leftCornerTop;
                }
                if (x == columns && y == rows || x == columns && y == -1)
                {
                    toInstantiate = null;
                    SpawnTile(new Vector3(rightCornerPos, rightCornerY),
                    cornerRight);
                    rightCornerY += (rows - leftCornerPos);
                    cornerRight = rightCornerTop;
                }

                if (toInstantiate != null)
                {
                    SpawnTile(new Vector3(x, y, 0f), toInstantiate);
                    if (toInstantiate.name.Contains("Floor"))
                    {
                        if (x % 2 == 0 && y % 2 == 0)
                        {
                            SpawnTile(new Vector3(x, y, 0f), danceFloorLeft);
                            SpawnTile(new Vector3(x + 1, y + 1, 0f),
                            danceFloorLeft);
                        }
                        if (x % 2 == 1 && y % 2 == 1)
                        {
                            SpawnTile(new Vector3(x - 1, y, 0f),
                            danceFloorRight);

                            SpawnTile(new Vector3(x, y - 1, 0f),
                            danceFloorRight);
                        }
                    }
                }
            }
        }
    }

    void SpawnTile(Vector3 position, GameObject toInstantiate)
    {
        GameObject instance =
            Instantiate(toInstantiate,
            new Vector3(position.x, position.y, 0f),
            Quaternion.identity) as
            GameObject;

        instance.transform.SetParent (tiles);

        //not best implementation to search for string
        if (instance.name.Contains("DanceFloorLeft"))
        {
            flashAnim.danceFloorLeft.Add(instance.GetComponent<Renderer>());
            instance.transform.SetParent (danceFloor);
        }
        if (instance.name.Contains("DanceFloorRight"))
        {
            flashAnim.danceFloorRight.Add(instance.GetComponent<Renderer>());
            instance.transform.SetParent (danceFloor);
        }
    }

    void LayoutObjectAtRand(GameObject[] tileArray, int min, int max)
    {
        int objectCount = Random.Range(min, max + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector2 randomPos = RandomPos();
            GameObject tileChoice =
                tileArray[Random.Range(0, tileArray.Length)];
            GameObject newObj =
                Instantiate(tileChoice, randomPos, Quaternion.identity);

            newObj.transform.SetParent (objects);

            INotation newNotation = newObj.GetComponent<INotation>();
            if (newNotation != null)
            {
                notations.Add (newNotation);
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

    public void SetupScene(int level)
    {
        ClearBoard();
        if (level % 2 != 0 && rowsMax != maxSize && firstTimeSetup)
        {
            rowsMin++;
            rowsMax++;
            columsMin++;
            columnsMax++;
            propCount.minimum++;
            propCount.maximum++;

            //if first time setup, save all of above to player prefs and then destroy all again (perhaps clear out notation etc.)
            //if not the first time setup, then load all of above plus whatever level you are on e.g. rowsMin = playerPrefs.getInt("Level" + level + "rowsMin")
        }

        if (randomSize)
        {
            columns = Random.Range(columsMin, columnsMax);
            if (columns % 2 != 0) columns += 1;
            rows = Random.Range(rowsMin, rowsMax);
            if (rows % 2 != 0) rows += 1;
        }
        else
        {
            columns = columnsMax;
            rows = rowsMax;
        }

        BoardSetup();
        InitialiseList();
        LayoutObjectAtRand(propTiles, propCount.minimum, propCount.maximum);

        // LayoutObjectAtRand(smashableTiles,
        // smashableCount.minimum,
        // smashableCount.maximum);
        if (spawnEnemies && level % 5 == 0)
        {
            spawnNotation = false;
            int enemyCount = (int) Mathf.FloorToInt(1 * Mathf.Sqrt(level));
            LayoutObjectAtRand (enemyTiles, enemyCount, enemyCount);
        }
        else
            spawnNotation = true;

        if (spawnNotation)
        {
            //(int) Mathf.Log(level, 2f); -- what is best method to increase notations over time & how are enemies introduced?
            int notationCount =
                (int) Mathf.FloorToInt(1.5f * Mathf.Sqrt(level));
            LayoutObjectAtRand (notationTiles, notationCount, notationCount);
        }

        GameObject newExit =
            Instantiate(exit,
            new Vector3(columns - 1, rows - 1, 0f),
            Quaternion.identity);
        newExit.transform.SetParent (objects);

        if (danceFloorHidden) danceFloor.gameObject.SetActive(false);
    }

    public void ClearBoard()
    {
        //Clear the board and increase sizes
        notations.Clear();
        flashAnim.danceFloorLeft.Clear();
        flashAnim.danceFloorRight.Clear();
        if (boardHolder != null) Destroy(boardHolder.gameObject);
    }

    public void HideShowDanceFloor(TextMeshProUGUI buttonText)
    {
        SoundController.PlaySound(SoundController.Sound.ButtonClick);

        if (!danceFloorHidden)
        {
            buttonText.text = "dance floor: off";
            danceFloorHidden = true;
            if (danceFloor != null) danceFloor.gameObject.SetActive(false);
        }
        else
        {
            buttonText.text = "dance floor: on";
            danceFloorHidden = false;
            if (danceFloor != null) danceFloor.gameObject.SetActive(true);
        }
    }

    public void RemoveNotationFromList(Transform pos)
    {
        foreach (INotation notation in notations.ToList())
        {
            if (notation.GetTransform().position == pos.position)
            {
                notations.Remove (notation);
                print(notations.Count);
            }
        }
    }

    //bool func returning notations empty or not
}
