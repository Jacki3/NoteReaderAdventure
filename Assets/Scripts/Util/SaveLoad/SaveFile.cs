using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveFile
{
    public string userIndex;

    public Vector3 playerPos;

    public int playerCoins;

    public int playerHealth = CoreGameElements.i.maxHealth;

    public int maxHealth = CoreGameElements.i.maxHealth;

    public int XP = 0;

    public int XPToNextLvl = 0;

    public int currentLevel = 0;

    public int overallScore;

    public int levelAt;

    public string defaultGameSeed;

    public bool firstRun = true;

    public bool danceFloorEnabled = false;

    public float musicVol = 1;

    public float SFXVol = 1;

    public float metroVol = 1;

    public float keysVol = 1;

    public bool usingColour = false;

    public List<Mission> allMissions = new List<Mission>();

    public TextureController.CustomisationItem[] savedCustomItems;

    public IventoryItem[] iventoryItems;

    public Sprite frontSprite;

    public Sprite backSprite;

    public Sprite sideSprite;

    public bool hasShield = false;

    public bool hasStrongShield = false;

    public List<PlayerSkills.SkillType>
        savedUnlockedSkills = new List<PlayerSkills.SkillType>();

    public int additionalHearts;

    public int livesLost;

    public int rhythmStreak;

    public int noteStreak;

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

        public int smashMin;

        public int smashMax;

        public bool HasMaxScore()
        {
            return score >= maxScore ? true : false;
        }
    }
}
