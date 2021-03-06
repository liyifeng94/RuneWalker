﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject[] EnemyPrefabs;

    public float[] EnemiesSpawnFrequency;

    public Transform PlayerSpawnMarker;
    public Transform EnemySpawnMarker;

    public Vector3 InitEnemyVelocity;

    private int _level = 1;
    private float _spawnFrequency;
    private GameObject _playerHolder;
    private Enemy _enemyInCombat;
    private LinkedList<Enemy> _enemiesHolder;
    private bool _inCombat;
    private float _lastSpawnTime;
    private Vector3 _enemyVelocity;
    private bool _holdSpawning;
    private bool _changingLevel;
    private ScrollingBackground _background;

    void Start()
    {
        GameManager.Instance.AddLevelManager(this);
        _background = GetComponent<ScrollingBackground>();
        LevelSetup();
        _lastSpawnTime = 0;
        UpdateEnemiesSpawnFrequency();
    }

    void Update()
    {
        float currentTime = Time.time;
        if (currentTime - _lastSpawnTime > _spawnFrequency)
        {
            if (_changingLevel)
            {
                return;
            }
            if (_holdSpawning)
            {
                if (_enemiesHolder.Count == 0)
                {
                    _holdSpawning = false;
                }
                else
                {
                    return;
                }
            }
            int enemyIndex = Random.Range(0, EnemyPrefabs.Length);
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
        _holdSpawning = true;

        _changingLevel = true;
        _background.NextLevel(this);

        UpdateEnemiesSpawnFrequency();

    }

    public void LevelBackgroundUpdated()
    {
        _changingLevel = false;
    }

    void LevelSetup()
    {
        _holdSpawning = false;
        _inCombat = false;
        _changingLevel = false;
        _enemyInCombat = null;
        _enemiesHolder = new LinkedList<Enemy>();
        SpawnPlayer();
        float currentTime = Time.time;
        _lastSpawnTime = currentTime;
        _enemyVelocity = InitEnemyVelocity;

        UpdateEnemiesSpawnFrequency();
    }

    // Spawns a enemy at SpawnLocation
    void SpawnEnemy(GameObject enemy)
    {
        GameObject newEnemy = (GameObject)Instantiate(enemy, EnemySpawnMarker.position, Quaternion.identity);
        Enemy newEnemyScript = newEnemy.GetComponent<Enemy>();
        newEnemyScript.ChangeVelocity(_enemyVelocity);
        _enemiesHolder.AddLast(newEnemyScript);
    }

    void SpawnPlayer()
    {
        _playerHolder = (GameObject)Instantiate(PlayerPrefab, PlayerSpawnMarker.position, Quaternion.identity);
    }

    public void ResolveCombat(Player.PlayerState playerAction)
    {
        if ((int) playerAction >= (int) (Enemy.AttackMove.NumOfMoves) || _inCombat == false || _enemyInCombat == null)
        {
            return;
        }

        if (_enemyInCombat == null)
        {
            return;
        }
        if (playerAction == Player.PlayerState.Special)
        {
            _enemyInCombat.Alive = false;
            return;
        }

        if ((int) playerAction == (int)_enemyInCombat.CurrentAttackMove)
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
        if (_playerHolder == null)
        {
            return;
        }
        Player playerObject = _playerHolder.GetComponent<Player>();
        playerObject.TakeDamage(1);
        _enemyInCombat.EndCombat(true);
    }

    public void SubmitPlayerAction(Player.PlayerState playerAction)
    {
        ResolveCombat(playerAction);
    }

    public void PlayerUseSpecial()
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

    public void Gameover()
    {
        
    }
}
