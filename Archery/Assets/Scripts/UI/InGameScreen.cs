using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InGameScreen : MonoBehaviour {
    GameManager gameManager;
    [SerializeField] GameObject prefabScore;
    [SerializeField] Vector3 prefabScorePosition;
    [SerializeField] Vector3 prefabScoreOffset;
    List<GameObject> prefabScoreList = new List<GameObject>();
    [SerializeField] GameObject gameOverScreen;
	// Use this for initialization
	void Start () {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        LevelEvents.ContinueToNextLevel += UpdateScore;
        LevelEvents.GameOver += EnableGameOverScreen;
        gameOverScreen.SetActive(false);
    }
    void UpdateScore()
    {
        GameObject prefabScoreInstance = Instantiate(prefabScore, this.transform);
        prefabScoreInstance.transform.position = prefabScorePosition + (prefabScoreOffset * (gameManager.roundNumber - 2));
        TextMeshProUGUI scoreText = prefabScoreInstance.GetComponent<TextMeshProUGUI>();
        scoreText.text = gameManager.roundScore[gameManager.roundNumber - 2].ToString();
        prefabScoreList.Add(prefabScoreInstance);
    }
    void RetryGame()
    {
        ClearPrefabScoreList();
        gameOverScreen.SetActive(false);
        LevelEvents.RaiseLevelEvent(LevelEvents.LevelEventType.RetryGame);
    }
    void ClearPrefabScoreList()
    {
        foreach (GameObject prefabScore in prefabScoreList)
            Destroy(prefabScore);
        prefabScoreList.Clear();
    }
    void EnableGameOverScreen()
    {
        gameOverScreen.SetActive(true);
    }
    private void OnDestroy() // avoid memory leaks
    {
        LevelEvents.ContinueToNextLevel -= UpdateScore;
    }
}