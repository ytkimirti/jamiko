using UnityEngine;
using System;
using System.Collections.Generic;

namespace OFK
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioSource DemoAudioSource;
        public static AudioManager Instance;

        private Dictionary<SoundData, AudioSource> _sourcesDict = new Dictionary<SoundData, AudioSource>();

        private void Awake()
        {
            Instance = this;
        }

        public void PlayFromDefaultAudioSource(SoundData sound)
        {
            if (!_sourcesDict.ContainsKey(sound))
            {
                AudioSource source = gameObject.AddComponent<AudioSource>();
                source.playOnAwake = false;
                _sourcesDict.Add(sound, source);
            }
            sound.PlayFromSource(_sourcesDict[sound]);
        }

        public static void PreviewSoundData(SoundData sound)
        {
            if (Application.isPlaying)
                sound.Play();
            CheckDemoAudioSource();
            sound.PlayFromSource(DemoAudioSource);
        }

        private static void CheckDemoAudioSource()
        {
            if (!DemoAudioSource)
            {
                GameObject newObject = new GameObject();
                newObject.name = "[ SFX PREVIEW ]";
                DemoAudioSource = newObject.AddComponent<AudioSource>();
                DemoAudioSource.playOnAwake = false;
            }
        }

        public static void PreviewAudioClip(AudioClip clip)
        {
            CheckDemoAudioSource();
            DemoAudioSource.clip = clip;
            DemoAudioSource.Play();
        }
    }
}