using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
public class TrajectoryPrediction : MonoBehaviour {
    private float velocity;
    private float angle;
    private float radianAngle;
    private float gravity = Mathf.Abs(Physics.gravity.y);
    [SerializeField] private int trajectoryResolution;
    [SerializeField] private BowMain bowMain;
    private LineRenderer lineRenderer;
    private GameManager gameManager;
    private void Start () {
        lineRenderer = GetComponent<LineRenderer>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        Assert.IsNotNull(bowMain);
    }
    private void Update()
    {
        if (gameManager == null)
            return;
        if (bowMain.loadedArrow != null && bowMain.BowPullStrength > bowMain.MinimumStrengthToShootArrow)
        {
            angle = -bowMain.transform.rotation.eulerAngles.x;
            velocity = bowMain.ShootForce * bowMain.BowPullStrength;
            lineRenderer.enabled = true;
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, bowMain.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
            transform.position = bowMain.loadedArrow.transform.position;
            RenderTrajectory();
        }
        else
            lineRenderer.enabled = false;
    }
    private void RenderTrajectory()
    {
        lineRenderer.positionCount = trajectoryResolution + 1;
        lineRenderer.SetPositions(GetTrajectoryPositions());
    }

    private Vector3[] GetTrajectoryPositions()
    {
        Vector3[] trajectoryPositions = new Vector3[trajectoryResolution + 1];
        radianAngle = Mathf.Deg2Rad * angle;
        float maxDistance = (velocity * velocity * Mathf.Sin(2 * radianAngle)) / gravity;
        for(int i = 0; i <= trajectoryResolution; i++)
        {
            float time = (float)i / (float)trajectoryResolution;
            trajectoryPositions[i] = CalculateTrajectoryPosition(time, maxDistance);
        }
        return trajectoryPositions;
    }

    private Vector3 CalculateTrajectoryPosition(float time, float maxDistance)
    {
        float horizontalPosition = time * maxDistance;
        float verticalPosition = horizontalPosition * Mathf.Tan(radianAngle) - ((horizontalPosition * horizontalPosition * gravity) / (2 * velocity * velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));
        return new Vector3(0f, verticalPosition, horizontalPosition);
    }
}
