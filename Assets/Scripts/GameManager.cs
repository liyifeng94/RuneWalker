using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public int LevelUpMultiplier = 4;

    public int BaseScore = 1;

    public int MaxScoreMultiplier = 100;

    //TODO: get player from text box
    public string PlayerName;

    [Serializable]
    public class HighScoreEntry
    {
        public string Name;
        public int Score;
        public string DateTime;
    }

    [Serializable]
    public class HighScoreBoard
    {
        public List<HighScoreEntry> HighScoreList;
    }

    // This should be sorted by time. 
    private HighScoreBoard _highScoreBoard;

    private int _currentLevel = 1;
    private int _playerKills = 0;
    private int _playerKillsCurrentLevel = 0;
    private int _score = 0;
    // Power meter for the special attack;
    private int _specialsUsed = 0;
    private int _specialMeter = 0;
    [HideInInspector] public int SpecialMeterMax = 2;
    public int MaxSpecialMultiplier = 2;

    //TODO: set up score Multiplier 
    private int _scoreMultiplier = 1;

    private LevelManager _gameLevelManager;

    public FibonacciSequence FibonacciSequence;

#if UNITY_EDITOR
    private const string HighScoreBoardPath = "Assets/Resources/GameJSONData/HighScore.json";
#elif UNITY_STANDALONE
    private const string HighScoreBoardPath = "GameData/Resources/GameJSONData/HighScore.json";
#else
    private const string HighScoreBoardPath = "HighScore.json";
#endif

    public void AddLevelManager(LevelManager gameLevelManager)
    {
        _gameLevelManager = gameLevelManager;
        InitGame();
    }

    public LevelManager GetLevelManager()
    {
        return _gameLevelManager;
    }

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

    public int GetScoreMultiplier()
    {
        return _scoreMultiplier;
    }

    public int GetSpecialMeter()
    {
        return _specialMeter;
    }

    public void ResetGame()
    {
        _gameLevelManager.InitLevel();
        _currentLevel = 1;
        _playerKills = 0;
        _playerKillsCurrentLevel = 0;
        _score = 0;
        _specialMeter = 0;
        SpecialMeterMax = 2;
        _specialsUsed = 0;
    }

    public bool HasSpecialAttack()
    {
        if (_specialMeter >= SpecialMeterMax)
        {
            return true;
        }
        return false;
    }

    public void UseSpecialAttack()
    {
        _scoreMultiplier = 1;
        _specialMeter = 0;
        ++_specialsUsed;
        SpecialMeterMax = MaxSpecialMultiplier * FibonacciSequence[_specialsUsed];
    }

    public void EnemiesKilled(int kills)
    {
        // group kills together even if the kills passed the level limit
        _score += kills*BaseScore*_currentLevel*_scoreMultiplier;
        _playerKills += kills;
        _playerKillsCurrentLevel += kills;
        _specialMeter += kills;

        if (_scoreMultiplier < MaxScoreMultiplier && _specialMeter == _scoreMultiplier + 1)
        {
            ++_scoreMultiplier;
        }

        // Increase game level because the level kill count 
        if (_playerKillsCurrentLevel > GetLevelUpKills())
        {
            IncreaseLevel();
        }

        Debug.Log("Score: " + _score);
        Debug.Log("Level: " + _currentLevel);
    }

    // Increase level
    void IncreaseLevel()
    {
        _playerKillsCurrentLevel = 0;
        ++_currentLevel;
        _gameLevelManager.SetLevel(_currentLevel);
    }

    // Get the kills needed to level
    int GetLevelUpKills()
    {
        return FibonacciSequence[_currentLevel] * 2;
    }

    void InitGame()
    {
        _currentLevel = 1;
        _playerKills = 0;
        _playerKillsCurrentLevel = 0;

        _score = 0;
    }

	// Use this for initialization
	void Awake ()
	{
        Debug.Log(DateTime.Now.ToString());
        // Make the game object a singleton 
        if (Instance == null)
	    {
	        Instance = this;
            FibonacciSequence = new FibonacciSequence();

        }
	    else
	    {
	        Destroy(gameObject);
	    }

        DontDestroyOnLoad(gameObject);

	    LoadHighScoreBoard();
	}
	
    // Sorted by time entered. Will need to be sorted by score in decreasing order.
    public HighScoreBoard GetHighScoreBoard()
    {
        return _highScoreBoard;
    }

    void UpdateHighScoreBoard()
    {
        HighScoreEntry newHighScoreEntry = new HighScoreEntry();
        newHighScoreEntry.Name = PlayerName;
        newHighScoreEntry.Score = _score;
        newHighScoreEntry.DateTime = DateTime.UtcNow.ToShortDateString();

        _highScoreBoard.HighScoreList.Add(newHighScoreEntry);
    }

    public void GameOver()
    {
        UpdateHighScoreBoard();
        enabled = false;
    }

    void LoadHighScoreBoard()
    {
        using (FileStream fs = new FileStream(HighScoreBoardPath, FileMode.Create))
        {
            using (StreamReader reader = new StreamReader(fs))
            {
                string jsonDataString = reader.ReadLine();
                _highScoreBoard = JsonUtility.FromJson<HighScoreBoard>(jsonDataString);
            }
        }

        if (_highScoreBoard == null)
        {
            _highScoreBoard = new HighScoreBoard();
        }
    }

    void SaveHighScoreBoard()
    {
        string jsonDataString = JsonUtility.ToJson(_highScoreBoard);
        using (FileStream fs = new FileStream(HighScoreBoardPath, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.Write(jsonDataString);
            }
        }
    }

    void OnApplicationQuit()
    {
        // Save high score on quit
        SaveHighScoreBoard();
    }
}
