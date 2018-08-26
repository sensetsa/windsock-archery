using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour {
    [SerializeField] private float secondsBeforeContinueToNextLevel = 2;
    private int[] roundScore = new int[10];
    public int[] RoundScore { get { return roundScore; } }
    private int roundNumber = 1;
    public int RoundNumber { get { return roundNumber; } }

    public enum GameState
    {
        PullArrowPhase, ShootArrowPhase
    }
    [HideInInspector] public GameState currentState = GameState.PullArrowPhase;
    private void Start () {
        for (int i = 0; i < roundScore.Length; i++)
        {
            roundScore[i] = 0;
        }
        BowEvents.ShootArrow += ChangeToShootArrowPhase;
        BowEvents.LandArrow += WaitForSecondsContinueToNextLevel;
        LevelEvents.RetryGame += PrepareNewGame;
	}
    private void WaitForSecondsContinueToNextLevel()
    {
        StartCoroutine(WaitForSecondsContinueToNextLevelEnumerator());
    }
    IEnumerator WaitForSecondsContinueToNextLevelEnumerator()
    {
        yield return new WaitForSeconds(secondsBeforeContinueToNextLevel);
        roundNumber += 1; // go to next round
        if (roundNumber > 10)
        {
            LevelEvents.RaiseLevelEvent(LevelEvents.LevelEventType.GameOver);
        }
        else
        {
            ChangeToPullArrowPhase();
            LevelEvents.RaiseLevelEvent(LevelEvents.LevelEventType.ContinueToNextLevel);
        }
    }
    private void PrepareNewGame()
    { 
        ChangeToPullArrowPhase();
        roundNumber = 1;
        for (int i = 0; i < roundScore.Length; i++)
        {
            roundScore[i] = 0;
        }
        LevelEvents.RaiseLevelEvent(LevelEvents.LevelEventType.ContinueToNextLevel);
    }
    public void AddScore(int addScore)
    {
        roundScore[roundNumber - 1] = addScore;
        LevelEvents.RaiseLevelEvent(LevelEvents.LevelEventType.AddScore);
    }
    private void ChangeToShootArrowPhase()
    {
        currentState = GameState.ShootArrowPhase;
    }
    private void ChangeToPullArrowPhase()
    {
        currentState = GameState.PullArrowPhase;
    }
    private void OnDestroy() // avoid memory leaks
    {
        BowEvents.ShootArrow -= ChangeToShootArrowPhase;
        BowEvents.LandArrow -= WaitForSecondsContinueToNextLevel;
        LevelEvents.RetryGame -= PrepareNewGame;
    }
}