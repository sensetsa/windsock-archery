using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
    Vector2 startPos;
    BowMain bowMain;
    Vector2 touchVector;
    GameManager gameManager;
	void Start () {
        bowMain = GetComponent<BowMain>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	}

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (gameManager.currentState == GameManager.GameState.PullArrowPhase)
                    {
                        startPos = touch.position;
                        bowMain.LoadArrow();
                    }
                    break;
                case TouchPhase.Moved:
                    if (gameManager.currentState == GameManager.GameState.PullArrowPhase)
                    {
                        touchVector = touch.position - startPos;
                        bowMain.RotateBowHorizontal(touch.position.x - startPos.x);
                        bowMain.RotateBowVertical(touch.position.y - startPos.y);
                        bowMain.PullArrow(startPos.y - touch.position.y);
                    }
                    break;
                case TouchPhase.Ended:
                    if (gameManager.currentState == GameManager.GameState.PullArrowPhase)
                    {
                        touchVector = touch.position - startPos;
                        bowMain.ShootArrow();
                    }
                    break;
            }
        }
    }
}