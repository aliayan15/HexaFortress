using System;
using UnityEngine;

namespace MyUtilities.Audio
{
    [Serializable]
    public class Sound
    {
        public SoundEnum Name;
        public AudioClip AudioClip;
        [Range(0, 1f)]
        public float Volume = 0.5f;
        [Range(0.1f, 3f)]
        public float Pitch = 1;
        public bool Loop;
        public bool PlayOnAwake;

        private AudioSource _audioSource;

        public void SetSource(AudioSource audioSource)
        {
            _audioSource = audioSource;
            _audioSource.clip = AudioClip;
            _audioSource.volume = Volume;
            _audioSource.pitch = Pitch;
            _audioSource.loop = Loop;
            _audioSource.playOnAwake = PlayOnAwake;

            if (PlayOnAwake)
                Play();
        }

        public void Play()
        {
            if (!_audioSource)
            {
                Debug.LogError("Can not play sound without setting audio source");
                return;
            }

            _audioSource.Play();
        }

        public void Stop()
        {
            if (!_audioSource)
            {
                Debug.LogError("Can not stop sound without setting audio source");
                return;
            }

            _audioSource.Stop();
        }

        public void Pause()
        {
            if (!_audioSource)
            {
                Debug.LogError("Can not play sound without setting audio source");
                return;
            }

            _audioSource.Pause();
        }
    }
}
