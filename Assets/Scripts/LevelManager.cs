using UnityEngine;
using System;
using System.Collections;

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
    private GameObject _enemyInCombat;
    private GameObject[] _enemiesHolder;
    private bool _inCombat;
    private float _combatStartTime = 0.0f;

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
        LevelChange();
        SpawnPlayer();
    }

    // Spawns a enemy at SpawnLocation
    void SpawnEnemy(GameObject enemy)
    {
        //TODO: spawn enemy

        //TODO: added enemy to enemies spawn array
    }

    void SpawnPlayer()
    {
        //TODO: spawn player
        _playerHolder = (GameObject)Instantiate(PlayerPrefab, PlayerSpawnLocation, Quaternion.identity);
        Debug.Log(_playerHolder);
    }

    void ResolveCombat(Player.PlayerState playerAction)
    {
        //TODO: check timer
        float timer = Time.time;

        //TODO: check if player action is valid
    }

    public void EnemyInCombat(GameObject enemyRef)
    {
        Debug.Log("EnemyInCombat");
        _enemyInCombat = enemyRef;
    }

    public void EnemyExitCombat(GameObject enemyRef)
    {
        Debug.Log("EnemyExitCombat");
        // Player take damage
        _playerHolder.SendMessage("TakeDamage", 1);
        _playerHolder.SendMessage("ExitCombat");

    }

    public void PlayerEnterCombat()
    {
        Debug.Log("PlayerEnterCombat");
        _combatStartTime = Time.time;
        _inCombat = true;
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
