using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour {
    public float targetDistanceToPlayer;
    [SerializeField] float secondsBeforeContinueToNextLevel = 5;
    public int[] roundScore;
    public int roundNumber = 1;
    public enum GameState
    {
        PullArrowPhase, ShootArrowPhase
    }
    [HideInInspector] public GameState currentState = GameState.PullArrowPhase;
	// Use this for initialization
	void Start () {
        roundNumber = 1;
        roundScore = new int[10];
        BowEvents.ShootArrow += ChangeToShootArrowPhase;
        BowEvents.ShootArrow += WaitForSecondsContinueToNextLevel;
        LevelEvents.SpawnTarget += GetTargetDistanceToPlayer;
        for(int i = 0; i < roundScore.Length; i++)
        {
            roundScore[i] = 0;
        }
        LevelEvents.RetryGame += PrepareNewGame;
	}
	public void GetTargetDistanceToPlayer()
    {
        SpawnManager spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        targetDistanceToPlayer = Vector3.Distance(spawnManager.currentObjectInstance.transform.position, player.transform.position);
    }
    public void WaitForSecondsContinueToNextLevel()
    {
        StartCoroutine(WaitForSecondsContinueToNextLevelEnumerator());
    }
    public IEnumerator WaitForSecondsContinueToNextLevelEnumerator()
    {
        yield return new WaitForSeconds(secondsBeforeContinueToNextLevel);
        roundNumber += 1; // go to next round
        if (roundNumber > 10)
        {
            roundNumber = 1;
            LevelEvents.RaiseLevelEvent(LevelEvents.LevelEventType.GameOver);
        }
        else
        {
            LevelEvents.RaiseLevelEvent(LevelEvents.LevelEventType.ContinueToNextLevel);
            ChangeToPullArrowPhase();
        }
    }
    public void PrepareNewGame()
    {
        LevelEvents.RaiseLevelEvent(LevelEvents.LevelEventType.ContinueToNextLevel);
        ChangeToPullArrowPhase();
        for(int i = 0; i < roundScore.Length; i++)
        {
            roundScore[i] = 0;
        }
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
        LevelEvents.SpawnTarget -= GetTargetDistanceToPlayer;
    }
    public void AddScore(int addScore)
    {
        roundScore[roundNumber - 1] = addScore;
    }
}