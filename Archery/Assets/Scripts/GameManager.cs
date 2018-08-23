using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour {
    [SerializeField] float secondsBeforeContinueToNextLevel = 5;
    [SerializeField] private float gameScore = 0;
    public float GameScore { get { return gameScore; } set { gameScore = value; } }
    public enum GameState
    {
        PullArrowPhase, ShootArrowPhase
    }
    public GameState currentState = GameState.PullArrowPhase;
	// Use this for initialization
	void Start () {
        BowEvents.ShootArrow += ChangeToShootArrowPhase;
        BowEvents.ShootArrow += WaitForSecondsContinueToNextLevel;
	}
	
    public void WaitForSecondsContinueToNextLevel()
    {
        StartCoroutine(WaitForSecondsContinueToNextLevelEnumerator());
    }
    public IEnumerator WaitForSecondsContinueToNextLevelEnumerator()
    {
        yield return new WaitForSeconds(secondsBeforeContinueToNextLevel);
        LevelEvents.RaiseLevelEvent(LevelEvents.LevelEventType.ContinueToNextLevel);
        ChangeToPullArrowPhase();
    }
    public void ChangeToShootArrowPhase()
    {
        currentState = GameState.ShootArrowPhase;
        GameStateEvents.RaiseLevelEvent(GameStateEvents.GameStateEventType.SwitchedToShootArrowPhase);
    }
    public void ChangeToPullArrowPhase()
    {
        currentState = GameState.PullArrowPhase;
        GameStateEvents.RaiseLevelEvent(GameStateEvents.GameStateEventType.SwitchedToLoadArrowPhase);
    }
    private void OnDestroy() // avoid memory leaks
    {
        BowEvents.ShootArrow -= ChangeToShootArrowPhase;
        BowEvents.ShootArrow -= WaitForSecondsContinueToNextLevel;
    }
}