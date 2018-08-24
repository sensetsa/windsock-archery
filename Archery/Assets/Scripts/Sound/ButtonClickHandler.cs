using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClickHandler : MonoBehaviour {
    private SoundManager soundManager; 
	// Use this for initialization
	void Start () {
        soundManager = SoundManager.Instance;
	}

    public void PlaySound(string clipName)
    {
        soundManager.PlaySound(clipName);
    }
}
