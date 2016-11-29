using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject[] EnemyPrefabs;

    public float[] EnemiesSpawnFrequency;
    public Vector3 PlayerSpawnLocation;
    public Vector3 EnemySpawnLocation;

    public GameObject[] ForegroundPrefabs;
    public GameObject[] BackgroundPrefabs;

    public Vector3 InitEnemyVelocity;

    private int _level = 1;
    private float _spawnFrequency;
    private GameObject _playerHolder;
    private Enemy _enemyInCombat;
    private LinkedList<Enemy> _enemiesHolder;
    private bool _inCombat;
    private float _lastSpawnTime;
    private Vector3 _enemyVelocity;

    void Start()
    {
        Debug.Log("Level started");
        GameManager.Instance.AddLevelManager(this);
        LevelSetup();
        _lastSpawnTime = 0;
        UpdateEnemiesSpawnFrequency();
    }

    void Update()
    {
        float currentTime = Time.time;
        if (currentTime - _lastSpawnTime > _spawnFrequency)
        {
            int enemyIndex = Random.Range(0, EnemyPrefabs.Length - 1);
            SpawnEnemy(EnemyPrefabs[enemyIndex]);
            _lastSpawnTime = currentTime;
        }
    }

    void UpdateEnemiesSpawnFrequency()
    {
        int frequencyIndex = Math.Min(_level - 1, EnemiesSpawnFrequency.Length-1);
        _spawnFrequency = EnemiesSpawnFrequency[frequencyIndex];
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
        _lastSpawnTime = currentTime;
        _enemyVelocity = InitEnemyVelocity;
    }

    // Spawns a enemy at SpawnLocation
    void SpawnEnemy(GameObject enemy)
    {
        GameObject newEnemy = (GameObject)Instantiate(enemy, EnemySpawnLocation, Quaternion.identity);
        Enemy newEnemyScript = newEnemy.GetComponent<Enemy>();
        newEnemyScript.ChangeVelocity(_enemyVelocity);
        _enemiesHolder.AddLast(newEnemyScript);
    }

    void SpawnPlayer()
    {
        _playerHolder = (GameObject)Instantiate(PlayerPrefab, PlayerSpawnLocation, Quaternion.identity);
    }

    public void ResolveCombat(Player.PlayerState playerAction)
    {
        if (playerAction == Player.PlayerState.Special)
        {
            PlayerUseSpecial();
            return;
        }

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
        _enemyVelocity = InitEnemyVelocity;
        _enemyVelocity.x -= (level * 0.2f);
        LevelChange();
    }

    public void InitLevel()
    {
        LevelSetup();
    }
}
