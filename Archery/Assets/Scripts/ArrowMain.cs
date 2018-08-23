using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
public class ArrowMain : MonoBehaviour {
    [HideInInspector] public VariableTransform variableTransform;
    Rigidbody rigidbody;
    [SerializeField] Collider arrowHeadCollider;

    public enum ArrowState
    {
        ShootArrow, LoadedToBow, LandedArrow
    }
    public ArrowState currentState = ArrowState.LoadedToBow;

    bool calledBowEventLandedArrowFlag = false;
    void Start () {
        Assert.IsNotNull(arrowHeadCollider);
        variableTransform = GetComponent<VariableTransform>();
        variableTransform.variableTransformReference = GetComponentInParent<VariableTransformReference>();
        rigidbody = GetComponent<Rigidbody>();

        LevelEvents.ContinueToNextLevel += this.DestroyObject;  //destroy arrow on loading next level
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Target")    //arrow scoring
        {
            currentState = ArrowState.LandedArrow;
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            currentState = ArrowState.LandedArrow;
        }
        if(!calledBowEventLandedArrowFlag)
        {
            calledBowEventLandedArrowFlag = true;
            BowEvents.RaiseBowEvent(BowEvents.BowEventType.LandArrow);
        }
    }
    public void DestroyObject()
    {
        Destroy(this.gameObject);
        LevelEvents.ContinueToNextLevel -= this.DestroyObject;
    }
    private void Update()
    {
        switch(currentState)
        {
            case ArrowState.LoadedToBow:
                break;
            case ArrowState.ShootArrow:
                if(rigidbody.velocity != Vector3.zero)
                    transform.rotation = Quaternion.LookRotation(rigidbody.velocity);   //transform bow rotation while flying
                break;
            case ArrowState.LandedArrow:
                break;
        }
    }
    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 10, Color.red);
    }
}