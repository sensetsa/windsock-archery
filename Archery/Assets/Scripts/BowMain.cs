﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
public class BowMain : MonoBehaviour {
    [SerializeField] float maxBowHorizontalRotation = 70f;
    [SerializeField] float maxBowVerticalRotation = 70f;
    [SerializeField] float minimumStrengthToShootArrow = 0.2f;
    public float MinimumStrengthToShootArrow { get { return minimumStrengthToShootArrow; } }
    float bowPullStrength = 0f;
    public float BowPullStrength { get { return bowPullStrength; } set { bowPullStrength = value; } }
    [SerializeField] float shootForce = 60f;
    public float ShootForce { get { return shootForce; } }
    [SerializeField] float bowPullStrengthDamp = 100f; //used to normalize pull vector to 0-1 range
    [SerializeField] float rotateDamp = 5f; //multiplier used to reduce bow rotation
    
    [SerializeField] GameObject prefabArrow;
    [HideInInspector] public GameObject loadedArrow;

    Vector3 initialPosition;
    Quaternion initialRotation;

    VariableTransformReference variableTransformReference;
    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        variableTransformReference = GetComponent<VariableTransformReference>();
        Assert.IsNotNull(prefabArrow);
        LevelEvents.ContinueToNextLevel += ResetTransform;  //reset rotation of bow after shoot phase
    }
    void Update () {
        if(variableTransformReference != null)
            variableTransformReference.ReferenceValue = BowPullStrength;
    }
    public void ShootArrow()
    {
        if (!(BowPullStrength > MinimumStrengthToShootArrow))   //prevent shooting if shooting strength is below minimum or no arrow is loaded.
            return;
        if (loadedArrow == null)
            return;

        loadedArrow.transform.parent = null;
        Rigidbody rigidbody = loadedArrow.GetComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.None;
        rigidbody.AddForce(transform.forward * ShootForce * BowPullStrength, ForceMode.Impulse);
        ArrowMain arrowMain = loadedArrow.GetComponent<ArrowMain>();
        arrowMain.currentState = ArrowMain.ArrowState.ShootArrow;
        arrowMain.variableTransform.enabled = false;
        BowPullStrength = 0;
        loadedArrow = null;
        BowEvents.RaiseBowEvent(BowEvents.BowEventType.ShootArrow);
    }
    public void LoadArrow()
    {
        if (loadedArrow == null)
        {
            loadedArrow = Instantiate(prefabArrow, this.transform);
            ArrowMain arrowMain = loadedArrow.GetComponent<ArrowMain>();
            arrowMain.currentState = ArrowMain.ArrowState.LoadedToBow;
            Rigidbody rigidbody = loadedArrow.GetComponent<Rigidbody>();
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            BowEvents.RaiseBowEvent(BowEvents.BowEventType.LoadArrow);
        }
    }
    public void PullArrow(float pullStrength)
    {
        BowPullStrength = Mathf.Clamp((pullStrength / bowPullStrengthDamp), 0, 1);
    }
    public void RotateBowHorizontal(float touchVector)
    {
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, Mathf.Clamp(touchVector / rotateDamp, -maxBowHorizontalRotation, maxBowHorizontalRotation) * -1, transform.rotation.eulerAngles.z));
    }
    public void RotateBowVertical(float touchVector)
    {
        transform.rotation = Quaternion.Euler(new Vector3(Mathf.Clamp(touchVector / rotateDamp, -maxBowVerticalRotation, maxBowVerticalRotation), transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
    }
    private void ResetTransform()
    {
        transform.rotation = initialRotation;
        transform.position = initialPosition;
    }
    private void OnDestroy()//prevent memory leaks
    {
        LevelEvents.ContinueToNextLevel -= ResetTransform;
    }
    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 10, Color.red);
    }
}
