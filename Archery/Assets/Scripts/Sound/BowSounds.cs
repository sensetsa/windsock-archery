using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowSounds : MonoBehaviour {
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Sound[] sounds;
    private BowMain bowMain;
    private bool bowPullStretchSoundIsPlaying = false;

    GameManager gameManager;
    private void Start () {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        bowMain = GetComponent<BowMain>();
        BowEvents.ShootArrow += ResetBowPullStretchSoundFlag;
        BowEvents.ShootArrow += PlayShootArrowSound;
	}

    private void Update () {
        if (gameManager.currentState == GameManager.GameState.PullArrowPhase)
        {
            if (bowMain.BowPullStrength > 0.5 && bowPullStretchSoundIsPlaying == false)
            {
                foreach(Sound sound in sounds)
                {
                    if (sound.audioClip.name == "bowPullStretch")
                    {
                        SoundExtensions.PasteAudioInformation(audioSource, sound);
                        audioSource.Play();
                        bowPullStretchSoundIsPlaying = true;
                        break;
                    }
                }
                
            }
            else if (bowMain.BowPullStrength < 0.5)
            {
                bowPullStretchSoundIsPlaying = false;
                audioSource.Stop();
            }
        }
	}
    private void ResetBowPullStretchSoundFlag()
    {
        bowPullStretchSoundIsPlaying = false;
    }
    private void PlayShootArrowSound()
    {
        foreach (Sound sound in sounds)
        {
            if (sound.audioClip.name == "shootArrow")
            {
                SoundExtensions.PasteAudioInformation(audioSource, sound);
                audioSource.Play();
                break;
            }
        }
    }
    
    private void OnDestroy()
    {
        BowEvents.ShootArrow -= ResetBowPullStretchSoundFlag;
        BowEvents.ShootArrow -= PlayShootArrowSound;
    }
}