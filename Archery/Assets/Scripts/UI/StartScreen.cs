using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class StartScreen : MonoBehaviour {
    [SerializeField] TextMeshProUGUI scoreText;
    private void Start()
    {
        if(scoreText != null)
        {
            scoreText.text = DataManager.Instance.HighScore.ToString();
        }
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}