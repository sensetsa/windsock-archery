using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour {
    [SerializeField] float secondsBeforeContinueToNextLevel = 2;
    [SerializeField] private float gameScore = 0;
    public float GameScore { get { return gameScore; } set { gameScore = value; } }
    public enum GameState
    {
        PullArrowPhase, ShootArrowPhase
    }
    public GameState currentState = GameState.PullArrowPhase;
	// Use this for initialization
	void Start () {
        BowEvents.LandArrow += WaitForSecondsContinueToNextLevel;
        BowEvents.ShootArrow += ChangeToShootArrowPhase;
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
    }
    public void ChangeToPullArrowPhase()
    {
        currentState = GameState.PullArrowPhase;
    }
    private void OnDestroy() // avoid memory leaks
    {
        BowEvents.LandArrow -= WaitForSecondsContinueToNextLevel;
        BowEvents.ShootArrow -= ChangeToShootArrowPhase;
    }
}