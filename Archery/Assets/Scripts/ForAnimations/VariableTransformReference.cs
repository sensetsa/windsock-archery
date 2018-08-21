using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableTransformReference : MonoBehaviour {
    float referenceValue;
    public float ReferenceValue
    {
        get { return referenceValue; }
        set { referenceValue = value; }
    }
}
