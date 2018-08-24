﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
public class InGameScreen : MonoBehaviour {
    [SerializeField] GameObject prefabScoreText;
    [SerializeField] Vector3 prefabScoreBasePosition;
    [SerializeField] Vector3 prefabScoreOffset;
    List<GameObject> prefabScoreList = new List<GameObject>();

    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject quickButtonScreen;
    [SerializeField] TextMeshProUGUI gameOverScore;

    GameManager gameManager;
    private void Start () {
        Assert.IsNotNull(gameOverScore);
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        Assert.IsNotNull(gameManager);
        gameOverScreen.SetActive(false);
        Assert.IsNotNull(gameOverScreen);
        quickButtonScreen.SetActive(false);
        Assert.IsNotNull(quickButtonScreen);
        LevelEvents.AddScore += UpdateScore;
        LevelEvents.GameOver += EnableGameOverScreen;
        LevelEvents.GameOver += SetGameOverScore;
        LevelEvents.ContinueToNextLevel += DisableQuickButtonScreenOnRetry;
    }
    private void UpdateScore()
    {
        GameObject prefabScoreInstance = Instantiate(prefabScoreText, this.transform);                                  //create a new score text and write the score
        prefabScoreInstance.transform.position = prefabScoreBasePosition + (prefabScoreOffset * (gameManager.RoundNumber - 1));
        TextMeshProUGUI scoreText = prefabScoreInstance.GetComponent<TextMeshProUGUI>();
        scoreText.text = gameManager.RoundScore[gameManager.RoundNumber - 1].ToString();
        prefabScoreList.Add(prefabScoreInstance);
    }
    public void ReturnToMenu(string mainMenu)
    {
        SceneManager.LoadScene(mainMenu);
    }
    public void RetryGame()
    {
        ClearPrefabScoreList();
        gameOverScreen.SetActive(false);
        LevelEvents.RaiseLevelEvent(LevelEvents.LevelEventType.RetryGame);
    }
    private void ClearPrefabScoreList()
    {
        foreach (GameObject prefabScore in prefabScoreList)
            Destroy(prefabScore);
        prefabScoreList.Clear();
    }
    public void EnableGameOverScreen()
    {
        gameOverScreen.SetActive(true);
    }
    public void EnableDisableQuickButtonMenu()
    {
        if (quickButtonScreen.activeSelf == false)
            quickButtonScreen.SetActive(true);
        else
            quickButtonScreen.SetActive(false);
    }
    private void DisableQuickButtonScreenOnRetry()
    {
        quickButtonScreen.SetActive(false);
    }
    private void SetGameOverScore()
    {
        int totalScore = 0;
        foreach(int score in gameManager.RoundScore)
        {
            totalScore += score;
        }
        gameOverScore.text = totalScore.ToString();
    }
    private void OnDestroy() // avoid memory leaks
    {
        LevelEvents.AddScore -= UpdateScore;
        LevelEvents.GameOver -= EnableGameOverScreen;
        LevelEvents.GameOver -= SetGameOverScore;
        LevelEvents.ContinueToNextLevel -= DisableQuickButtonScreenOnRetry;
    }
}