using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public LevelManager GameLevelManager;
    public int[] LevelKillCount;
    public int BaseScore = 1;

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

    private HighScoreBoard _highScoreBoard;

    private int _currentLevel = 1;
    private int _playerKills = 0;
    private int _playerKillsCurrentLevel = 0;
    private int _score = 0;

#if UNITY_EDITOR
    private const string HighScoreBoardPath = "Assets/Resources/GameJSONData/HighScore.json";
#elif UNITY_STANDALONE
    private const string HighScoreBoardPath = "GameData/Resources/GameJSONData/HighScore.json";
#else
    private const string HighScoreBoardPath = "HighScore.json";
#endif

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
        Debug.Log(DateTime.Now.ToString());
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

	    LoadHighScoreBoard();

        InitGame();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

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
        string jsonDataString = "";
        using (FileStream fs = new FileStream(HighScoreBoardPath, FileMode.Create))
        {
            using (StreamReader Reader = new StreamReader(fs))
            {
                jsonDataString = Reader.ReadLine();
                _highScoreBoard = JsonUtility.FromJson<HighScoreBoard>(jsonDataString);
            }
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
