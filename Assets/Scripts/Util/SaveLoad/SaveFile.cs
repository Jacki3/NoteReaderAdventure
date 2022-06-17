using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFile
{
    public Vector3 playerPos;

    public int playerCoins;

    public int playerHealth = CoreGameElements.i.maxHealth;

    public int levelAt;

    public string defaultGameSeed;

    public bool firstRun = true;

    public bool danceFloorEnabled = false;

    public List<Board> boards = new List<Board>();

    [Serializable]
    public class Board
    {
        public int level;

        public int score;

        public int maxScore;

        public int rowsMins;

        public int rowsMax;

        public int columsMin;

        public int columnsMax;

        public int propsMin;

        public int propsMax;

        public bool HasMaxScore()
        {
            return score >= maxScore ? true : false;
        }
    }
}
