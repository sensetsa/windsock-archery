using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
public class TrajectoryPrediction : MonoBehaviour {
    float velocity;
    float angle;
    float radianAngle;
    float gravity = Mathf.Abs(Physics.gravity.y);
    [SerializeField] int trajectoryResolution;
    [SerializeField] BowMain bowMain;
    
    LineRenderer lineRenderer;
    void Start () {
        lineRenderer = GetComponent<LineRenderer>();
        Assert.IsNotNull(bowMain);
    }
    private void Update()
    {

        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, bowMain.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
        if(bowMain.loadedArrow != null)
            transform.position = bowMain.loadedArrow.transform.position;
        if (bowMain.bowPullStrength > bowMain.minimumStrengthToShootArrow)
        {
            angle = -bowMain.transform.rotation.eulerAngles.x;
            velocity = bowMain.shootForce * bowMain.bowPullStrength;
            lineRenderer.enabled = true;
            RenderTrajectory();
        }
        else
        {
            lineRenderer.enabled = false;
        }
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
