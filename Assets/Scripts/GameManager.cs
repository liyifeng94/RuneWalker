using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

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

    // This is sorted by time. 
    private HighScoreBoard _highScoreBoard;

#if UNITY_EDITOR
    private const string HighScoreBoardPath = "Assets/Resources/GameJSONData/HighScore.json";
#elif UNITY_STANDALONE
    private const string HighScoreBoardPath = "GameData/Resources/GameJSONData/HighScore.json";
#else
    private const string HighScoreBoardPath = "HighScore.json";
#endif

    public class GameState
    {
        //Game info
        public int CurrentLevel = 1;
        public int PlayerKills = 0;
        public int Score = 0;

        // Power meter for the special attack;
        public int SpecialsUsed = 0;
        public int SpecialMeter = 0;

        //Score multiplier
        public int ScoreMultiplier = 1;

        public void ResetState()
        {
            CurrentLevel = 0;
            PlayerKills = 0;
            Score = 0;

            SpecialsUsed = 0;
            SpecialMeter = 0;

            ScoreMultiplier = 1;
        }
    }

    public int LevelUpMultiplier = 4;

    public int BaseScore = 1;

    public int MaxScoreMultiplier = 100;

    [HideInInspector] public int SpecialMeterMax = 2;
    public int MaxSpecialMultiplier = 2;

    public FibonacciSequence FibonacciSequence;



    private LevelManager _gameLevelManager;
    private int _playerKillsCurrentLevel = 0;
    private GameState _currentGameState;

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
        return _currentGameState.CurrentLevel;
    }

    public int GetPlayerKills()
    {
        return _currentGameState.PlayerKills;
    }

    public int GetScore()
    {
        return _currentGameState.Score;
    }

    public int GetScoreMultiplier()
    {
        return _currentGameState.ScoreMultiplier;
    }

    public int GetSpecialMeter()
    {
        return _currentGameState.SpecialMeter;
    }

    public void ResetGame()
    {
        _gameLevelManager.InitLevel();
        _currentGameState.ResetState();
        _playerKillsCurrentLevel = 0;
        SpecialMeterMax = 2;
    }

    public bool HasSpecialAttack()
    {
        if (GetSpecialMeter() >= SpecialMeterMax)
        {
            return true;
        }
        return false;
    }

    public void UseSpecialAttack()
    {
        _currentGameState.ScoreMultiplier = 1;
        _currentGameState.SpecialMeter = 0;
        SpecialMeterMax = MaxSpecialMultiplier * FibonacciSequence[++_currentGameState.SpecialsUsed];
    }

    public void EnemiesKilled(int kills)
    {
        // group kills together even if the kills passed the level limit
        _currentGameState.Score += kills*BaseScore* _currentGameState.CurrentLevel* _currentGameState.ScoreMultiplier;
        _currentGameState.PlayerKills += kills;
        _playerKillsCurrentLevel += kills;
        _currentGameState.SpecialMeter += kills;

        // Increase score multiplier
        if (_currentGameState.ScoreMultiplier < MaxScoreMultiplier)
        {
            ++_currentGameState.ScoreMultiplier;
        }

        // Increase game level because the level kill count 
        if (_playerKillsCurrentLevel > GetLevelUpKills())
        {
            IncreaseLevel();
        }
    }

    // Increase level
    void IncreaseLevel()
    {
        _playerKillsCurrentLevel = 0;
        _gameLevelManager.SetLevel(++_currentGameState.CurrentLevel);
    }

    // Get the kills needed to level
    int GetLevelUpKills()
    {
        return FibonacciSequence[_currentGameState.CurrentLevel] * 2;
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
            FibonacciSequence = new FibonacciSequence();
            _currentGameState = new GameState();
            LoadHighScoreBoard();
        }
	    else
	    {
	        Destroy(gameObject);
	    }

        DontDestroyOnLoad(gameObject);
	}
	
    // Sorted by time entered. Will need to be sorted by score in decreasing order.
    public HighScoreBoard GetHighScoreBoard()
    {
        return _highScoreBoard;
    }

    public void UpdateHighScoreBoard(string playerName)
    {
        HighScoreEntry newHighScoreEntry = new HighScoreEntry();
        newHighScoreEntry.Name = playerName;
        newHighScoreEntry.Score = _currentGameState.Score;
        newHighScoreEntry.DateTime = DateTime.UtcNow.ToShortDateString();

        _highScoreBoard.HighScoreList.Add(newHighScoreEntry);
    }

    public void GameOver()
    {
        //TODO: change level to scoreboard
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
