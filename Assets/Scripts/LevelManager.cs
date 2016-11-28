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

    private int _level = 1;
    private Transform _levelHolder;
    private float _spawnFrequency;
    private GameObject _playerHolder;
    private Enemy _enemyInCombat;
    private LinkedList<Enemy> _enemiesHolder;
    private bool _inCombat;
    private float lastSpawnTime;

    void Start()
    {
        Debug.Log("Level started");
        GameManager.Instance.AddLevelManager(this);
        LevelSetup();
        lastSpawnTime = 0;
        UpdateEnemiesSpawnFrequency();
    }

    void Update()
    {
        //TODO: spawn enemies
        float currentTime = Time.time;
        if (currentTime - lastSpawnTime > _spawnFrequency)
        {
            SpawnEnemy(EnemyPrefabs[0]);
            lastSpawnTime = currentTime;
        }
    }

    void UpdateEnemiesSpawnFrequency()
    {
        //TODO: change to dynamically changed from code
        //_spawnFrequency = EnemiesSpawnFrequency[_level-1];
        _spawnFrequency = EnemiesSpawnFrequency[0];
    }

    void LevelChange()
    {
        //TODO: set background

        //TODO: update spawn frequency
        UpdateEnemiesSpawnFrequency();
    }

    void LevelSetup()
    {
        _inCombat = false;
        _enemyInCombat = null;
        _enemiesHolder = new LinkedList<Enemy>();
        LevelChange();
        SpawnPlayer();
        float currentTime = Time.time;
        SpawnEnemy(EnemyPrefabs[0]);
        lastSpawnTime = currentTime;
    }

    // Spawns a enemy at SpawnLocation
    void SpawnEnemy(GameObject enemy)
    {
        //TODO: spawn enemy
        GameObject newEnemy = (GameObject)Instantiate(enemy, EnemySpawnLocation, Quaternion.identity);
        Enemy newEnemyScript = newEnemy.GetComponent<Enemy>();
        _enemiesHolder.AddLast(newEnemyScript);

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
        if (playerAction == Player.PlayerState.Special)
        {
            PlayerUseSpecial();
            return;
        }

        //TODO: check if player action is valid
        if ((int) playerAction >= (int) (Enemy.AttackMove.NumOfMoves) || _inCombat == false)
        {
            return;
        }

        Enemy target = _enemyInCombat.GetComponent<Enemy>();
        if (target == null)
        {
            return;
        }
        if ((int) playerAction == (int)target.CurrentAttackMove)
        {
            _enemyInCombat.EndCombat(false);
            GameManager.Instance.EnemiesKilled(1);
            _enemiesHolder.Remove(_enemyInCombat);
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
        Player playerObject = _playerHolder.GetComponent<Player>();
        playerObject.TakeDamage(1);
        _enemyInCombat.EndCombat(true);
    }

    public void SubmitPlayerAction(Player.PlayerState playerAction)
    {
        ResolveCombat(playerAction);
    }

    void PlayerUseSpecial()
    {
        Debug.Log("Use Special");
        foreach (var enemy in _enemiesHolder)
        {
            enemy.EndCombat(false);
            GameManager.Instance.EnemiesKilled(1);
        }
        _enemiesHolder.Clear();
        GameManager.Instance.UseSpecialAttack();
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
