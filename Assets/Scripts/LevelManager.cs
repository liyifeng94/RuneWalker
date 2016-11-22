using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject[] EnemyPrefabs;

    public float[] EnemiesSpawnFrequency;
    public Vector3 PlayerSpawnLocation;
    public Vector3 EnemySpawnLocation;

    public GameObject[] ForegroundPrefabs;
    public GameObject[] BackgroundPrefabs;

    // Decision Time in seconds
    public float DecisionTimer = 2;


    private int _level = 1;
    private Transform _levelHolder;
    private float _spawnFrequency;
    private GameObject _playerHolder;
    private Enemy _enemyInCombat;
    private List<GameObject> _enemiesHolder;
    private bool _inCombat;

    void Start()
    {
        Debug.Log("Level started");
        GameManager.Instance.AddLevelManager(this);
        LevelSetup();
    }

    void UpdateEnemiesSpawnFrequency()
    {
        //TODO: change to dynamically changed from code
        _spawnFrequency = EnemiesSpawnFrequency[_level-1];
    }

    void LevelChange()
    {
        //TODO: set background

        //TODO: update spawn frequency
    }

    void LevelSetup()
    {
        _inCombat = false;
        _enemyInCombat = null;
        _enemiesHolder = new List<GameObject>();
        LevelChange();
        SpawnPlayer();
        SpawnEnemy(EnemyPrefabs[0]);
    }

    // Spawns a enemy at SpawnLocation
    void SpawnEnemy(GameObject enemy)
    {
        //TODO: spawn enemy
        GameObject newEnemy = (GameObject)Instantiate(enemy, EnemySpawnLocation, Quaternion.identity);
        _enemiesHolder.Add(newEnemy);

        //TODO: added enemy to enemies spawn array
    }

    void SpawnPlayer()
    {
        //TODO: spawn player
        _playerHolder = (GameObject)Instantiate(PlayerPrefab, PlayerSpawnLocation, Quaternion.identity);
        Debug.Log(_playerHolder);
    }

    public void ResolveCombat(Player.PlayerState playerAction)
    {
        //TODO: check if player action is valid
        if ((int) playerAction >= (int) (Enemy.AttackMove.NumOfMoves) || _inCombat == false)
        {
            return;
        }

        Enemy target = _enemyInCombat.GetComponent<Enemy>();
        if ((int) playerAction == (int)target.CurrentAttackMove)
        {
            _enemyInCombat.EndCombat(false);
            GameManager.Instance.EnemiesKilled(1);
        }
        else
        {
            EnemyExitCombat();
        }
    }

    public void EnemyInCombat(Enemy enemyRef)
    {
        Debug.Log("EnemyInCombat");
        _enemyInCombat = enemyRef;
        _inCombat = true;
    }

    public void EnemyExitCombat()
    {
        _inCombat = false;
        if (_enemyInCombat.Alive == false)
        {
            return;
        }

        Debug.Log("EnemyExitCombat");
        // Player take damage
        _playerHolder.SendMessage("TakeDamage", 1);
        _enemyInCombat.EndCombat(true);
    }

    public void SubmitPlayerAction(Player.PlayerState playerAction)
    {
        ResolveCombat(playerAction);
    }

    public void SetLevel(int level)
    {
        _level = level;
        LevelChange();
    }

    public void InitLevel()
    {
        _levelHolder = new GameObject("GameLevel").transform;
        LevelSetup();
    }
}
