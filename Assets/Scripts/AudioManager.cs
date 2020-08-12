using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(AudioManager)) as AudioManager;
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
        cache = new Dictionary<string, AudioClip>();
    }

    private Dictionary<string, AudioClip> cache;
    public AudioClip LoadAudioClip(string path)
    {
        if (cache.ContainsKey(path))
        {
            return cache[path];
        }
        AudioClip clip = Resources.Load<AudioClip>(path);
        if (clip != null)
        {
            cache.Add(path, clip);
        }
        return clip;
    }
    public void PlayAudioClip(AudioClip audioClip, float fromSec = 0, float toSec = 1)
    {
        audioSource.clip = audioClip;
        audioSource.time = fromSec;
        audioSource.Play();
        audioSource.SetScheduledEndTime(AudioSettings.dspTime + (toSec - fromSec));
    }
    public void PlayButtonClickAudio()
    {
        audioSource.clip = LoadAudioClip("effect/ButtonClickSound");
        audioSource.time = 0;
        audioSource.Play();
    }
}