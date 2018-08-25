using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
    private Vector2 startTouchPosition;

    private BowMain bowMain;
    private GameManager gameManager;
    private void Start () {
        bowMain = GetComponent<BowMain>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	}

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (gameManager != null && gameManager.currentState == GameManager.GameState.PullArrowPhase)
                    {
                        startTouchPosition = touch.position;
                        bowMain.LoadArrow();
                    }
                    break;
                case TouchPhase.Moved:
                    if (gameManager != null && gameManager.currentState == GameManager.GameState.PullArrowPhase && bowMain.loadedArrow != null)
                    {
                        bowMain.RotateBowHorizontal(touch.position.x - startTouchPosition.x);
                        bowMain.RotateBowVertical(touch.position.y - startTouchPosition.y);
                        bowMain.PullArrow(startTouchPosition.y - touch.position.y);
                    }
                    break;
                case TouchPhase.Ended:
                    if (gameManager != null && gameManager.currentState == GameManager.GameState.PullArrowPhase)
                    {
                        bowMain.ShootArrow();
                    }
                    break;
            }
        }
    }
}