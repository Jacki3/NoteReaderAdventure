using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public enum SpawnState
    {
        spawning,
        waiting,
        counting
    }

    [System.Serializable]
    public class Wave
    {
        public string name;

        public EnemyAI[] enemies;

        public int count;

        public float rate;

        public Transform[] spawnPoints;
    }

    public Transform playerPos;

    public Wave[] waves;

    public float timeBetweenWaves;

    public float waveCountDown;

    private SpawnState spawnState = SpawnState.counting;

    private int nextWave = 0;

    public List<EnemyAI> enemies = new List<EnemyAI>();

    private int totalEnemies;

    void StartSpawning()
    {
        waveCountDown = timeBetweenWaves;
    }

    void Update()
    {
        if (spawnState == SpawnState.waiting)
        {
            if (!EnemiesLeft())
            {
                WaveCompleted();
                return;
            }
            else
                return;
        }
        if (waveCountDown <= 0)
        {
            if (spawnState != SpawnState.spawning)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountDown -= Time.deltaTime;
        }
    }

    void WaveCompleted()
    {
        spawnState = SpawnState.counting;
        waveCountDown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            GameStateController.PauseGame();
            UIController
                .UpdateTextUI(UIController.UITextComponents.arenaWinText,
                "Level Complete!");
        }
        else
        {
            nextWave++;
        }
    }

    bool EnemiesLeft() => totalEnemies > 0;

    IEnumerator SpawnWave(Wave _wave)
    {
        totalEnemies = _wave.count;
        spawnState = SpawnState.spawning;
        for (int i = 0; i < _wave.count; i++)
        {
            int randIndex = Random.Range(0, _wave.enemies.Length);
            var randEnemy = _wave.enemies[randIndex];
            SpawnEnemy(randEnemy, _wave.spawnPoints);
            yield return new WaitForSeconds(1 / _wave.rate);
        }
        spawnState = SpawnState.waiting;

        yield break;
    }

    void SpawnEnemy(EnemyAI enemy, Transform[] spawns)
    {
        if (spawns.Length <= 0) Debug.LogError("No Spawns!");
        int randIndex = Random.Range(0, spawns.Length);
        Transform randSpawn = spawns[randIndex];
        var newEnemy = Instantiate(enemy);
        newEnemy.transform.position = randSpawn.position;
        newEnemy.playerPos = playerPos;
        newEnemy.enemySpawner = this;
        foreach (Notation
            notation
            in
            newEnemy.GetComponentsInChildren<Notation>()
        )
        {
            notation.arenaMode = true;
        }
        enemies.Add (enemy);
    }

    public void RemoveEnemy(EnemyAI enemy)
    {
        enemies.Remove (enemy);
        totalEnemies--;
    }
}
