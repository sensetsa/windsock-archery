using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSounds : MonoBehaviour {
    [SerializeField] private Sound hitTargetSound;
    [SerializeField] private AudioSource audioSource;
	// Use this for initialization
	void Start () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        SoundExtensions.PasteAudioInformation(audioSource, hitTargetSound);
        audioSource.Play();
    }
}
