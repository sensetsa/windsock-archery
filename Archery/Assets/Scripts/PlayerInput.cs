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
                    startPos = touch.position;
                    bowMain.LoadArrow();
                    break;
                case TouchPhase.Moved:
                    touchVector = touch.position - startPos;
                    //bowMain.RotateBow(touchVector);
                    bowMain.PullArrow(touchVector);
                    break;
                case TouchPhase.Ended:
                    touchVector = touch.position - startPos;
                    bowMain.ShootArrow(touchVector);
                    break;
            }
        }
        if(Input.GetMouseButtonDown(0))
        {
            if(gameManager.currentState == GameManager.GameState.PullArrowPhase)
                bowMain.LoadArrow();
        }
        if(Input.GetMouseButtonDown(1))
        {
            if (gameManager.currentState == GameManager.GameState.PullArrowPhase)
                bowMain.ShootArrow(Vector2.zero); // TODO actual shoot arrow calculations
        }
        if(Input.GetKey("q"))
        {
            bowMain.RotateBow(20);
        }
        if(Input.GetKey("e"))
        {
            bowMain.RotateBow(-20);
        }
    }
}