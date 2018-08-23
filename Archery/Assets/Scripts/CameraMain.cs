using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
public class CameraMain : MonoBehaviour {
    Vector3 initialPosition;
    Quaternion initialRotation;
    float initialFOV;
    [SerializeField] Vector3 cameraFollowArrowDistanceOffset = Vector3.zero;
    [SerializeField] Vector3 cameraFollowArrowRotationOffset = Vector3.zero;

    [SerializeField]BowMain bowMain;
    GameObject activeArrow;

    GameManager gameManager;
	void Start () {
        Assert.IsNotNull(bowMain);
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        Camera camera = GetComponent<Camera>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialFOV = camera.fieldOfView;
        BowEvents.LoadArrow += GetActiveArrow;              // get active arrow when arrow is loaded
        GameStateEvents.SwitchedToLoadArrowPhase += SwitchViewToBehindPlayer; // place camera behind player during the load phase
        GameStateEvents.SwitchedToShootArrowPhase += SwitchViewToFollowArrow; // make camera follow arrow during the shoot phase
	}
	
	void LateUpdate () {
        switch (gameManager.currentState)
        {
            case GameManager.GameState.ShootArrowPhase:
                transform.position = activeArrow.transform.position + cameraFollowArrowDistanceOffset;
                transform.rotation = Quaternion.Euler(cameraFollowArrowRotationOffset);
                break;
            default:
                break;
        }
	}
    public void SwitchViewToBehindPlayer()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        Camera camera = GetComponent<Camera>();
        camera.fieldOfView = initialFOV;
    }
    public void SwitchViewToFollowArrow()
    {
        Camera camera = GetComponent<Camera>();
        camera.fieldOfView = 80f;
    }
    public void GetActiveArrow()
    {
        activeArrow = bowMain.loadedArrow;
    }
    private void OnDestroy() //avoid memory leaks
    {
        BowEvents.LoadArrow -= GetActiveArrow;              // get active arrow when arrow is loaded
        GameStateEvents.SwitchedToLoadArrowPhase -= SwitchViewToBehindPlayer;
        GameStateEvents.SwitchedToShootArrowPhase -= SwitchViewToFollowArrow;
    }
}