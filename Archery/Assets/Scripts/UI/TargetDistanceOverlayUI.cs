using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TargetDistanceOverlayUI : MonoBehaviour {
    [SerializeField] TextMeshProUGUI targetDistanceText;
    public GameObject targetDistanceTextObject;
    [HideInInspector] public float targetDistanceFromPlayer;
	// Use this for initialization
	void Start () {
		if(targetDistanceText != null)
            targetDistanceText.text = Mathf.Round(targetDistanceFromPlayer).ToString() + "m";
	}
}
