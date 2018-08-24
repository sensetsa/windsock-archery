using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
public class BowMain : MonoBehaviour {
    public float minimumStrengthToShootArrow = 0.2f;
    [Range(0f, 1f)] public float bowPullStrength = 0f;
    [SerializeField] float bowPullStrengthMultiplier = 100f; //used to normalize pull vector to 0-1 range
    [SerializeField] float rotateDamp = 5f; //multiplier used to reduce bow rotation
    public float shootForce = 30f;
    [SerializeField] GameObject prefabArrow;
    [HideInInspector] public GameObject loadedArrow;

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
    public void ShootArrow()
    {
        if (!(bowPullStrength > minimumStrengthToShootArrow))
            return;
        if (loadedArrow == null)
            return;

        loadedArrow.transform.parent = null;
        Rigidbody rigidbody = loadedArrow.GetComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.None;
        rigidbody.AddForce(transform.forward * shootForce * bowPullStrength, ForceMode.Impulse);
        ArrowMain arrowMain = loadedArrow.GetComponent<ArrowMain>();
        arrowMain.currentState = ArrowMain.ArrowState.ShootArrow;
        arrowMain.variableTransform.enabled = false;
        bowPullStrength = 0;
        BowEvents.RaiseBowEvent(BowEvents.BowEventType.ShootArrow);
        loadedArrow = null;
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
        bowPullStrength = Mathf.Clamp((pullStrength / bowPullStrengthMultiplier), 0, 1);
    }
    public void RotateBowHorizontal(float touchVector)
    {
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, Mathf.Clamp(touchVector / rotateDamp, -70, 70) * -1, transform.rotation.eulerAngles.z));
    }
    public void RotateBowVertical(float touchVector)
    {
        transform.rotation = Quaternion.Euler(new Vector3(Mathf.Clamp(touchVector / rotateDamp, -70, 70), transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
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
