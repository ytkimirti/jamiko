using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] musicClips;
    [SerializeField] private AudioSource musicAudioSource;

    public static MusicManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void PlayMusic()
    {
        musicAudioSource.clip = musicClips[Random.Range(0, musicClips.Length)];
        musicAudioSource.Play();
        musicAudioSource.volume = 1;
    }

    public void StopMusic()
    {
        musicAudioSource.DOFade(0, 0.5f);
    }
}
