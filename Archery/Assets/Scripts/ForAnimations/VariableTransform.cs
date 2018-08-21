using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class VariableTransform : MonoBehaviour {
    public VariableTransformReference variableTransformReference;
    float referenceVal;
    [SerializeField] float multiplierStrength = 1;
    enum TransformType
    {
        MoveX, MoveY, MoveZ, RotateX, RotateY, RotateZ, ScaleX, ScaleY, ScaleZ
    }
    [SerializeField] TransformType transformType;
    Vector3 initialPosition;
    Vector3 initialRotation;
    Vector3 initialScale;
	void Start () {
        initialPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        initialRotation = new Vector3(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
        initialScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
	}
    private void LateUpdate()
    {
        if(variableTransformReference != null)
            referenceVal = variableTransformReference.ReferenceValue;
        switch (transformType)
        {
            case TransformType.MoveX:
                this.transform.localPosition = new Vector3(initialPosition.x + referenceVal * multiplierStrength, transform.localPosition.y, transform.localPosition.z);
                break;
            case TransformType.MoveY:
                this.transform.localPosition = new Vector3(transform.localPosition.x, initialPosition.y + referenceVal * multiplierStrength, transform.localPosition.z);
                break;
            case TransformType.MoveZ:
                this.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, initialPosition.z + referenceVal * multiplierStrength);
                break;
            case TransformType.RotateX:
                this.transform.localRotation = Quaternion.Euler(new Vector3(initialRotation.x + referenceVal * multiplierStrength, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z));
                break;
            case TransformType.RotateY:
                this.transform.localRotation = Quaternion.Euler(new Vector3(transform.localRotation.eulerAngles.x, initialRotation.y + referenceVal * multiplierStrength, transform.localRotation.eulerAngles.z));
                break;
            case TransformType.RotateZ:
                this.transform.localRotation = Quaternion.Euler(new Vector3(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, initialRotation.z + referenceVal * multiplierStrength));
                break;
            case TransformType.ScaleX:
                break;
            case TransformType.ScaleY:
                break;
            case TransformType.ScaleZ:
                break;
        }
    }
}
