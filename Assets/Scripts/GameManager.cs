using UnityEngine;
using System;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public LevelManager GameLevelManager;
    public int[] LevelKillCount;
    public int BaseScore = 1;

    private int _currentLevel = 1;
    private int _playerKills = 0;
    private int _playerKillsCurrentLevel = 0;
    private int _score = 0;

    public int GetCurrentLevel()
    {
        return _currentLevel;
    }

    public int GetPlayerKills()
    {
        return _playerKills;
    }

    public int GetScore()
    {
        return _score;
    }

    public void ResetGame()
    {
        GameLevelManager.InitLevel();
        _currentLevel = 1;
        _playerKills = 0;
        _playerKillsCurrentLevel = 0;

        //TODO: update high score
        _score = 0;
    }

    public void AddKill()
    {
        _score += BaseScore*_currentLevel;
        ++_playerKills;
        ++_playerKillsCurrentLevel;

        // Increase game level because the level kill count 
        if (_playerKillsCurrentLevel == GetLevelUpKills())
        {
            IncreaseLevel();
        }
    }

    // Increase level
    void IncreaseLevel()
    {
        _playerKillsCurrentLevel = 0;
        ++_currentLevel;
        GameLevelManager.SetLevel(_currentLevel);
    }

    // Get the kills needed to level
    int GetLevelUpKills()
    {
        // If the next level is not configured
        if (_currentLevel > LevelKillCount.Length)
        {
            return -1;
        }

        //TODO: Change to dynamically changed from code. Instead of fixed amount.
        return LevelKillCount[_currentLevel];
    }

    void InitGame()
    {
        ResetGame();
    }

	// Use this for initialization
	void Awake ()
	{
        // Make the game object a singleton 
	    if (Instance == null)
	    {
	        Instance = this;
	    }
	    else
	    {
	        Destroy(gameObject);
	    }

        DontDestroyOnLoad(gameObject);

        GameLevelManager = GetComponent<LevelManager>();
	    InitGame();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void GameOver()
    {
        enabled = false;
    }
}
