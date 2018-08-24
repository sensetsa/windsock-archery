using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Sound {
    public AudioClip audioClip;
    public float volume;
    public float pitch;
    public bool loop;
}
[System.Serializable]
public class EventSounds : Sound
{
    [HideInInspector] public AudioSource audioSource;
}
public class SoundExtensions
{
    public static void PasteAudioInformation(AudioSource audioSource, Sound sound)
    {
        audioSource.clip = sound.audioClip;
        audioSource.volume = sound.volume;
        audioSource.pitch = sound.pitch;
        audioSource.loop = sound.loop;
    }
}
