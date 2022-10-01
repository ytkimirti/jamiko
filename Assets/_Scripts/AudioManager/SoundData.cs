using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;


namespace OFK
{
    [CreateAssetMenu(fileName = "SoundData", menuName = "New/SoundData")]
    public class SoundData : ScriptableObject
    {
        [Range(0, 1f)] public float volume = 1f;

        public AudioClip[] clips;

        public bool playIfNotPlaying = false;
        public bool randomPitch = false;

        [ShowIf("randomPitch"), MinMaxSlider(0.2f, 2.4f)]
        public Vector2 pitchRange = new Vector2(1, 1);

        [HideIf("randomPitch"), Range(0.2f, 2.4f)]
        public float pitch = 1f;

        [Button]
        public void Preview()
        {
            AudioManager.PreviewSoundData(this);
        }

        public void Play()
        {
            if (!AudioManager.Instance)
            {
                Debug.LogError("No audio manager instance found!");
                return;
            }
            AudioManager.Instance.PlayFromDefaultAudioSource(this);
        }

        public void PlayFromSource(AudioSource source)
        {
            if (clips.Length == 0)
                return;
            if (playIfNotPlaying && source.isPlaying)
                return;
            source.clip = clips[Random.Range(0, clips.Length)];
            source.volume = volume;
            
            if (randomPitch)
                source.pitch = Random.Range(pitchRange.x, pitchRange.y);
            else
                source.pitch = pitch;
            
            source.Play();
        }
    }
}