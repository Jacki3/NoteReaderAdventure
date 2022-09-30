using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public enum SpawnState
    {
        spawning,
        waiting,
        counting,
        complete
    }

    [System.Serializable]
    public class Wave
    {
        public string name;

        public RhythmEnemy[] enemies;

        public int count;

        public float rate;

        public List<Transform> spawnPoints = new List<Transform>();
    }

    public Wave[] waves;

    public float timeBetweenWaves;

    public float waveCountDown;

    private SpawnState spawnState = SpawnState.counting;

    private int nextWave = 0;

    public List<RhythmEnemy> enemies = new List<RhythmEnemy>();

    public static int currentWave;

    public int totalEnemies;

    private bool waitToGo;

    private int difficultyAddition = 0;

    private TimeSpan startTime;

    private TimeSpan endTime;

    private int diffWave;

    private void OnEnable()
    {
        Invoke("StartSpawning", 5);
        CoreGameElements.i.arenaMode = true;
        MusicGenController.DisableMusic();

        startTime = DateTime.Now.TimeOfDay;
    }

    void StartSpawning()
    {
        waveCountDown = timeBetweenWaves;
        waitToGo = true;
    }

    void Update()
    {
        if (spawnState == SpawnState.waiting)
        {
            if (!EnemiesLeft())
            {
                StartCoroutine(WaveComplete());
                return;
            }
            else
                return;
        }
        if (waveCountDown <= 0)
        {
            if (
                spawnState != SpawnState.spawning &&
                spawnState != SpawnState.complete
            )
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else if (waitToGo)
        {
            UIController
                .UpdateTextUI(UIController.UITextComponents.arenaWinText,
                ((int) waveCountDown + 1).ToString());
            waveCountDown -= Time.deltaTime;
        }
    }

    IEnumerator WaveComplete()
    {
        spawnState = SpawnState.complete;
        currentWave++;
        int highestWave = CoreGameElements.i.gameSave.highestArenaWave;
        if (currentWave > highestWave)
            CoreGameElements.i.gameSave.highestArenaWave = currentWave;
        UIController
            .UpdateTextUI(UIController.UITextComponents.highestWaveStat,
            currentWave.ToString());

        int currentScore = ScoreController.GetScoreStatic();
        int highScore = CoreGameElements.i.gameSave.arenaHighScore;
        if (currentScore > highScore)
        {
            CoreGameElements.i.gameSave.arenaHighScore = currentScore;
            StartMenu.UpdateArenaHighScore();
        }
        UIController
            .UpdateTextUI(UIController.UITextComponents.arenaWinText,
            "wave " + (currentWave) + " beaten!");
        yield return new WaitForSeconds(2f);
        WaveCompleted();
    }

    void WaveCompleted()
    {
        spawnState = SpawnState.counting;
        waveCountDown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            difficultyAddition += 2; //hard code

            nextWave = 0;

            //
            //if you want to complete the wave system
            //
            // GameStateController.PauseGame(true);
            // UIController
            //     .UpdateTextUI(UIController.UITextComponents.arenaWinText,
            //     "Arena Complete!");
        }
        else
        {
            nextWave++;
            diffWave++;
            if (diffWave == 16) diffWave = 1;
            DifficultyPicker.SetArenaDifficulty (diffWave);
        }
    }

    bool EnemiesLeft() => totalEnemies > 0;

    IEnumerator WaitToSpawn(bool reset)
    {
        yield return new WaitForSeconds(1.5f);
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        totalEnemies = _wave.count + difficultyAddition;
        spawnState = SpawnState.spawning;

        List<Transform> newSpawns = new List<Transform>();
        newSpawns.AddRange(_wave.spawnPoints);

        UIController
            .UpdateTextUI(UIController.UITextComponents.arenaWinText,
            "Wave " + (currentWave + 1));
        yield return new WaitForSeconds(2);
        UIController
            .UpdateTextUI(UIController.UITextComponents.arenaWinText, "");
        for (int i = 0; i < _wave.count + difficultyAddition; i++)
        {
            int randIndex = Random.Range(0, _wave.enemies.Length);
            var randEnemy = _wave.enemies[randIndex];

            int randIndexSpawn = Random.Range(0, newSpawns.Count);
            Transform randSpawn = newSpawns[randIndexSpawn];
            newSpawns.Remove (randSpawn);

            if (newSpawns.Count <= 0) newSpawns.AddRange(_wave.spawnPoints);

            SpawnEnemy (randEnemy, randSpawn);
            yield return new WaitForSeconds(1 / _wave.rate);
        }
        spawnState = SpawnState.waiting;

        yield break;
    }

    void SpawnEnemy(RhythmEnemy enemy, Transform spawn)
    {
        if (spawn == null) Debug.LogError("No Spawn!");

        var newEnemy = Instantiate(enemy);
        newEnemy.transform.position = spawn.position;
        newEnemy.enemySpawner = this;
        enemies.Add (newEnemy);
    }

    public void RemoveEnemy(RhythmEnemy enemy)
    {
        enemies.Remove (enemy);
        totalEnemies--;
    }

    public void ResetSpawner()
    {
        waitToGo = false;
        spawnState = SpawnState.counting;
        nextWave = 0;
        currentWave = 0;
        foreach (RhythmEnemy enemy in enemies) Destroy(enemy.gameObject);
        UIController
            .UpdateTextUI(UIController.UITextComponents.arenaWinText, "");
    }

    private void OnDisable()
    {
        ResetSpawner();
        CoreGameElements.i.arenaMode = false;
        endTime = DateTime.Now.TimeOfDay;

        TimeSpan diff = endTime - startTime;

        CoreGameElements.i.gameSave.timeInArena += diff;
    }
}
