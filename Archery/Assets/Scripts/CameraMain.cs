using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
public class CameraMain : MonoBehaviour {
    Vector3 initialPosition;
    Quaternion initialRotation;
    [SerializeField] Vector3 cameraFollowArrowDistanceOffset = Vector3.zero;
    [SerializeField] Vector3 cameraFollowArrowRotationOffset = Vector3.zero;

    [SerializeField]BowMain bowMain;
    GameObject activeArrow;

    GameManager gameManager;
	void Start () {
        Assert.IsNotNull(bowMain);
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        BowEvents.LoadArrow += GetActiveArrow;              // get active arrow when arrow is loaded
	}
	
	void LateUpdate () {
        switch (gameManager.currentState)
        {
            case GameManager.GameState.PullArrowPhase:
                transform.position = initialPosition;
                transform.rotation = initialRotation;
                break;
            case GameManager.GameState.ShootArrowPhase:
                transform.position = activeArrow.transform.position + cameraFollowArrowDistanceOffset;
                transform.rotation = Quaternion.Euler(cameraFollowArrowRotationOffset);
                break;
        }
	}
    public void GetActiveArrow()
    {
        activeArrow = bowMain.loadedArrow;
    }
    private void OnDestroy() //avoid memory leaks
    {
        BowEvents.LoadArrow -= GetActiveArrow;              // get active arrow when arrow is loaded
    }
}