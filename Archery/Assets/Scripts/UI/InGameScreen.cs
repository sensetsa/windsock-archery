using System.Collections;
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
    [SerializeField] GameObject scoreBoard;
    [SerializeField] TextMeshProUGUI gameOverScore;
    [SerializeField] TextMeshProUGUI rotationUIText;
    [SerializeField] TextMeshProUGUI pullStrengthUIText;

    GameManager gameManager;
    BowMain bowMain;
    private void Start () {
        
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        bowMain = GameObject.FindGameObjectWithTag("Player").GetComponent<BowMain>();
        gameOverScreen.SetActive(false);
        quickButtonScreen.SetActive(false);
        Assert.IsNotNull(bowMain);
        Assert.IsNotNull(gameOverScore);
        Assert.IsNotNull(gameManager);
        Assert.IsNotNull(gameOverScreen);
        Assert.IsNotNull(quickButtonScreen);
        Assert.IsNotNull(rotationUIText);
        Assert.IsNotNull(pullStrengthUIText);
        LevelEvents.AddScore += UpdateScore;
        LevelEvents.GameOver += EnableGameOverScreen;
        LevelEvents.GameOver += SetGameOverScore;
        LevelEvents.ContinueToNextLevel += DisableQuickButtonScreenOnRetry;
    }
    private void Update()
    {
        int rotationUIAngle = (360 - (int)Mathf.Round(bowMain.transform.localRotation.eulerAngles.x));
        if(rotationUIAngle != 360)
            rotationUIText.text = rotationUIAngle.ToString();
        else
            rotationUIText.text = "0";
        pullStrengthUIText.text = (Mathf.Round(bowMain.BowPullStrength * 100)).ToString();
    }
    private void UpdateScore()
    {
        GameObject prefabScoreInstance = Instantiate(prefabScoreText, scoreBoard.transform);                                  //create a new score text and write the score
        prefabScoreInstance.transform.localPosition = prefabScoreBasePosition + (prefabScoreOffset * (gameManager.RoundNumber - 1));
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