using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    private static SoundManager _instance;
    public static SoundManager Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    [SerializeField] private EventSounds[] eventSounds;
    Dictionary<string, EventSounds> eventSoundsDictionary = new Dictionary<string, EventSounds>();

    private void Start () {
		foreach(EventSounds eventSound in eventSounds)
        {
            eventSound.audioSource = gameObject.AddComponent<AudioSource>();
            eventSound.audioSource.clip = eventSound.audioClip;
            eventSound.audioSource.volume = eventSound.volume;
            eventSound.audioSource.pitch = eventSound.pitch;
            eventSound.audioSource.loop = eventSound.loop;
            eventSoundsDictionary.Add(eventSound.audioClip.name, eventSound);
        }
	}
	public void PlaySound(string clipName)
    {
        if (eventSoundsDictionary.ContainsKey(clipName))
        {
            EventSounds playSound = eventSoundsDictionary[clipName];
            playSound.audioSource.Play();
        }
    }
}
