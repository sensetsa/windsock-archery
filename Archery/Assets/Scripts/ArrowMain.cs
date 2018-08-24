using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
public class ArrowMain : MonoBehaviour {
    [HideInInspector] public VariableTransform variableTransform;
    private Rigidbody rigidbody;
    [SerializeField] private Collider arrowHeadCollider;

    public enum ArrowState
    {
        ShootArrow, LoadedToBow, LandedArrow
    }
    public ArrowState currentState = ArrowState.LoadedToBow;
    private bool arrowLandFlag = false;

    GameManager gameManager;
    private void Start () {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        variableTransform = GetComponent<VariableTransform>();
        variableTransform.variableTransformReference = GetComponentInParent<VariableTransformReference>();
        rigidbody = GetComponent<Rigidbody>();
        Assert.IsNotNull(arrowHeadCollider);
        Assert.IsNotNull(rigidbody);
        LevelEvents.ContinueToNextLevel += this.DestroyObject;  //destroy arrow on loading next level
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (arrowLandFlag)
            return;
        arrowLandFlag = true;
        if (collision.gameObject.tag == "Target")    //arrow scoring
        {
            currentState = ArrowState.LandedArrow;
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            BowEvents.RaiseBowEvent(BowEvents.BowEventType.LandArrow);
        }
        else if (gameManager != null)       //if arrow landed on ground
        {
            gameManager.AddScore(0);
            currentState = ArrowState.LandedArrow;
            BowEvents.RaiseBowEvent(BowEvents.BowEventType.LandArrow);
        }
    }
    private void DestroyObject()
    {
        Destroy(this.gameObject);
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
    private void OnDestroy()
    {
        LevelEvents.ContinueToNextLevel -= this.DestroyObject;
    }
    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 10, Color.red);
    }
}