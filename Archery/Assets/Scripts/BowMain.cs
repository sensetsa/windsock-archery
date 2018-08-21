using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
public class BowMain : MonoBehaviour {
    [Range(0f, 1f)] public float bowPullStrength = 0f;
    [SerializeField] float shootForce = 10f;
    [SerializeField] GameObject prefabArrow;
    public GameObject loadedArrow;

    VariableTransformReference variableTransformReference;

    Vector3 initialPosition;
    Quaternion initialRotation;
    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        Assert.IsNotNull(prefabArrow);
        variableTransformReference = GetComponent<VariableTransformReference>();

        LevelEvents.ContinueToNextLevel += ResetTransform;
    }
    void Update () {
        if(variableTransformReference != null)
            variableTransformReference.ReferenceValue = bowPullStrength;
    }
    public void ShootArrow(Vector2 touchVector)
    {
        if (loadedArrow != null)
        {
            loadedArrow.transform.parent = null;
            Rigidbody rigidbody = loadedArrow.GetComponent<Rigidbody>();
            rigidbody.constraints = RigidbodyConstraints.None;
            rigidbody.AddForce(transform.forward * shootForce, ForceMode.Impulse);
            ArrowMain arrowMain = loadedArrow.GetComponent<ArrowMain>();
            arrowMain.currentState = ArrowMain.ArrowState.ShootArrow;
            arrowMain.variableTransform.enabled = false;
            bowPullStrength = 0;
            BowEvents.RaiseBowEvent(BowEvents.BowEventType.ShootArrow);
            loadedArrow = null;
        }
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
    public void PullArrow(Vector2 touchVector)
    {

    }
    public void RotateBow(float direction)
    {
        transform.RotateAround(transform.position, Vector3.up, direction * Time.deltaTime);
    }
    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 10, Color.red);
    }
    public void ResetTransform()
    {
        transform.rotation = initialRotation;
        transform.position = initialPosition;
    }
    private void OnDestroy()//prevent memory leaks
    {
        LevelEvents.ContinueToNextLevel -= ResetTransform;
    }
}
