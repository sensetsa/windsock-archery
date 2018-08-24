using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour {
    private static DataManager _instance;
    public static DataManager Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
        highScore = PlayerPrefs.GetInt(highScoreKey, 0);
        LevelEvents.GameOver += SaveHighScore;
    }
    string highScoreKey = "HighScore";
    int highScore = 0;
    public int HighScore { get { return highScore; } }

    void SaveHighScore()
    {
        GameManager gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        if(gameManager != null)
        {
            int totalScore = 0;
            foreach(int score in gameManager.RoundScore)
            {
                totalScore += score;
            }
            if (totalScore > highScore)
                highScore = totalScore;
        }
        PlayerPrefs.SetInt(highScoreKey, highScore);
        PlayerPrefs.Save();
    }
    private void OnDestroy()
    {
        LevelEvents.GameOver -= SaveHighScore;
    }
}