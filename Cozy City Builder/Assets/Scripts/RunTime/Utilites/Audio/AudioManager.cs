using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MyUtilities.Audio
{
    public enum SoundEnum
    {
        MENULOOP,
        GAMELOOP
    }

    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;
        public Sound[] Sounds;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);

            foreach (var sound in Sounds)
            {
                sound.SetSource(gameObject.AddComponent<AudioSource>());
            }
        }

        
        public void Play(SoundEnum audioName)
        {
            var sound = GetSound(audioName);
            sound?.Play();
        }
        public void Stop(SoundEnum audioName)
        {
            var sound = GetSound(audioName);
            sound?.Stop();
        }
        public void Pause(SoundEnum audioName)
        {
            var sound = GetSound(audioName);
            sound?.Pause();
        }
        public void StopAllSoundExceptOne(SoundEnum audioName)
        {
            foreach (var sound in Sounds)
            {
                if (sound.Name == audioName)
                {
                    sound?.Play();
                    return;
                }

                sound?.Stop();
            }
        }

        public Sound GetSound(SoundEnum audioName)
        {
            var sound = Array.Find(Sounds, s => s.Name == audioName);
            if (sound == null)
            {
                Debug.LogError("Sound Not Exists");
                return null;
            }
            return sound;
        }


    }
}
