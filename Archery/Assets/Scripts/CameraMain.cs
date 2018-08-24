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
    bool cameraFollowArrow = false;

	private void Start () {
        Assert.IsNotNull(bowMain);
        Camera camera = GetComponent<Camera>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialFOV = camera.fieldOfView;
        BowEvents.LoadArrow += GetActiveArrow;              // get active arrow when arrow is loaded
        LevelEvents.ContinueToNextLevel += SwitchViewToBehindPlayer; // place camera behind player during the load phase
        BowEvents.ShootArrow += SwitchViewToFollowArrow; // make camera follow arrow during the shoot phase
	}
	private void LateUpdate () {
        if (cameraFollowArrow)
        {
            transform.position = activeArrow.transform.position + cameraFollowArrowDistanceOffset;
            transform.rotation = Quaternion.Euler(cameraFollowArrowRotationOffset);
        }
    }
    private void SwitchViewToBehindPlayer()
    {
        cameraFollowArrow = false;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        Camera camera = GetComponent<Camera>();
        camera.fieldOfView = initialFOV;
    }
    private void SwitchViewToFollowArrow()
    {
        cameraFollowArrow = true;
        Camera camera = GetComponent<Camera>();
        camera.fieldOfView = 80f;
    }
    private void GetActiveArrow()
    {
        activeArrow = bowMain.loadedArrow;
    }
    private void OnDestroy() //avoid memory leaks
    {
        BowEvents.LoadArrow -= GetActiveArrow;              // get active arrow when arrow is loaded
        LevelEvents.ContinueToNextLevel -= SwitchViewToBehindPlayer; // place camera behind player during the load phase
        BowEvents.ShootArrow -= SwitchViewToFollowArrow; // make camera follow arrow during the shoot phase
    }
}