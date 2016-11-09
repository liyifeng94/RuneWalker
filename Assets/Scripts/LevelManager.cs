using UnityEngine;
using System;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject[] EnemyPrefabs;

    public float[] EnemiesSpawnFrequency;
    public Vector3 PlaySpawnLocation;
    public Vector3 EnemySpawnLocation;

    public GameObject[] ForegroundPrefabs;
    public GameObject[] BackgroundPrefabs;


    private int _level = 1;
    private Transform _levelHolder;
    private float _spawnFrequency;

    void UpdateEnemiesSpawnFrequency()
    {
        //TODO: change to dynamically changed from code
        _spawnFrequency = EnemiesSpawnFrequency[_level-1];
    }

    void UpdateLevel()
    {
        //TODO: set background

        //TODO: update spawn frequency
    }

    void LevelSetup()
    {
        UpdateLevel();

        //TODO: spawn player
    }

    // Spawns a enemy at SpawnLocation
    void SpawnEnemy(GameObject enemy)
    {
        //TODO: spawn enemy
    }

    public void SetLevel(int level)
    {
        _level = level;
        UpdateLevel();
    }

    public void InitLevel()
    {
        _levelHolder = new GameObject("GameLevel").transform;
        LevelSetup();
    }
}
