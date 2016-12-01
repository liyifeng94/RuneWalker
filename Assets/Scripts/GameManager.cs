using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public int LevelUpMultiplier = 4;

    public int BaseScore = 1;

    public int MaxScoreMultiplier = 100;

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

        public HighScoreBoard()
        {
            HighScoreList = new List<HighScoreEntry>();
        }
    }

    // This should be sorted by time. 
    private HighScoreBoard _highScoreBoard;

#if UNITY_EDITOR
    private const string HighScoreBoardPath = "Assets/Resources/GameJSONData/Runtime/HighScore.json";
#elif UNITY_STANDALONE
    private const string HighScoreBoardPath = "GameData/Resources/GameJSONData/HighScore.json";
#else
    private const string HighScoreBoardPath = "HighScore.json";
#endif

    public class GameState
    {
        public int CurrentLevel = 1;
        public int PlayerKills = 0;
        public int PlayerKillsCurrentLevel = 0;
        public int Score = 0;

        // Power meter for the special attack;
        public int SpecialsUsed = 0;
        public int SpecialMeter = 0;
        public int SpecialMeterMax = 2;

        public int ScoreMultiplier = 1;

        public void Reset()
        {
            CurrentLevel = 1;
            PlayerKills = 0;
            PlayerKillsCurrentLevel = 0;
            Score = 0;

            // Power meter for the special attack;
            SpecialsUsed = 0;
            SpecialMeter = 0;

            ScoreMultiplier = 1;
        }
    }

    private GameState _gameState;
    
    public int MaxSpecialMultiplier = 2;

    private LevelManager _gameLevelManager;

    public FibonacciSequence FibonacciSequence;

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
        return _gameState.CurrentLevel;
    }

    public int GetPlayerKills()
    {
        return _gameState.PlayerKills;
    }

    public int GetScore()
    {
        return _gameState.Score;
    }

    public int GetScoreMultiplier()
    {
        return _gameState.ScoreMultiplier;
    }

    public int GetSpecialMeter()
    {
        return _gameState.SpecialMeter;
    }

    public int GetSpecialMeterMax()
    {
        return _gameState.SpecialMeterMax;
    }

    public void ResetGame()
    {
        _gameLevelManager.InitLevel();
        _gameState.Reset();
    }

    public bool HasSpecialAttack()
    {
        if (_gameState.SpecialMeter >= _gameState.SpecialMeterMax)
        {
            return true;
        }
        return false;
    }

    public void UseSpecialAttack()
    {
        _gameState.ScoreMultiplier = 1;
        _gameState.SpecialMeter = 0;
        _gameState.SpecialMeterMax = MaxSpecialMultiplier * FibonacciSequence[++_gameState.SpecialsUsed];
    }

    public void EnemiesKilled(int kills)
    {
        // group kills together even if the kills passed the level limit
        _gameState.Score += kills*BaseScore* _gameState.CurrentLevel * _gameState.ScoreMultiplier;
        _gameState.PlayerKills += kills;
        _gameState.PlayerKillsCurrentLevel += kills;
        _gameState.SpecialMeter += kills;

        if (_gameState.ScoreMultiplier < MaxScoreMultiplier && _gameState.SpecialMeter == _gameState.ScoreMultiplier + 1)
        {
            ++_gameState.ScoreMultiplier;
        }

        // Increase game level because the level kill count 
        if (_gameState.PlayerKillsCurrentLevel > GetLevelUpKills())
        {
            IncreaseLevel();
        }
    }

    // Increase level
   void IncreaseLevel()
    {
        _gameState.PlayerKillsCurrentLevel = 0;
        
        _gameLevelManager.SetLevel(++_gameState.CurrentLevel);
    }

    // Get the kills needed to level
    int GetLevelUpKills()
    {
        return FibonacciSequence[_gameState.CurrentLevel] * 3;
    }

    void InitGame()
    {
        _gameState.Reset();
    }

	// Use this for initialization
	void Awake ()
	{
        // Make the game object a singleton 
        if (Instance == null)
	    {
	        Instance = this;
            FibonacciSequence = new FibonacciSequence();
            _gameState = new GameState();
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
        newHighScoreEntry.Score = _gameState.Score;
        newHighScoreEntry.DateTime = DateTime.UtcNow.ToString();

        _highScoreBoard.HighScoreList.Add(newHighScoreEntry);
		_highScoreBoard.HighScoreList.Sort (SortByScore);
		_highScoreBoard.HighScoreList.Reverse ();
    }


	static int SortByScore(HighScoreEntry score1, HighScoreEntry score2){
		return score1.Score.CompareTo (score2.Score);
	}

    public void GameOver()
    {
        //enabled = false;
        _gameLevelManager.Gameover();
        SceneManager.LoadScene("PlayerNameLevel");
    }

    void LoadHighScoreBoard()
    {
        _highScoreBoard = null;
        Directory.CreateDirectory(Path.GetDirectoryName(HighScoreBoardPath));
        using (FileStream fs = new FileStream(HighScoreBoardPath, FileMode.OpenOrCreate))
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
        Directory.CreateDirectory(Path.GetDirectoryName(HighScoreBoardPath));
        using (FileStream fs = new FileStream(HighScoreBoardPath, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.Write(jsonDataString);
            }
        }
    }

	public void SetPlayerNameAndHighScore(String name)
	{
		PlayerName = name;
		UpdateHighScoreBoard (PlayerName);
		SaveHighScoreBoard ();
		SceneManager.LoadScene("HighScore");
	}

    void OnApplicationQuit()
    {
        // Save high score on quit
        SaveHighScoreBoard();
    }
}
