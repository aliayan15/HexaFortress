using MyUtilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Managers
{
    public class AudioManager : SingletonMono<AudioManager>
    {

        [HideInInspector]
        public List<AudioPoolSource> AudioSources = new List<AudioPoolSource>();
        [Header("Settings")]
        [SerializeField] private AudioMixer audioMixer;

        [Space(15)]
        [SerializeField]
        private GameObject sourceObject;
        [Header("Audio Clips")]
        [SerializeField] Sound[] sounds;
        [Space(10)]
        [SerializeField] private AudioPoolSource defauldSound;
        [SerializeField] private AudioPoolSource musicSound;


        #region Play Audio
        public void ClearAudioList()
        {
            AudioSources.Clear();
        }
        public void PlayMusic(SoundTypes s)
        {
            Sound sound = GetClip(s);
            AudioClip soundClip = sound.GetClip();
            musicSound.PlayClip(soundClip, sound.Valume, 0, sound.AudioMixerGroup, 200);
        }
        public void StopMusic()
        {
            musicSound.StopClip();
        }

        public void PlayBtnSound()
        {
            float pitchMin = 1 - 0.02f;
            float pitchMax = 1 + 0.02f;
            Sound sound = GetClip(SoundTypes.BtnClick);
            AudioClip soundClip = sound.GetClip();
            defauldSound.PlayClip(soundClip, sound.Valume, pitchMin, pitchMax, sound.AudioMixerGroup, 128);

        }

        public void Play2DSound(SoundTypes s, int priority = 128)
        {
            float pitchMin = 1 - 0.1f;
            float pitchMax = 1 + 0.1f;
            Sound sound = GetClip(s);
            AudioClip soundClip = sound.GetClip();
            defauldSound.PlayClip(soundClip, sound.Valume, pitchMin, pitchMax, sound.AudioMixerGroup, priority);

        }

        /// <summary>
        /// Play sound with enum.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="pos"></param>
        /// <param name="mValume"></param>
        /// <param name="priority"></param>
        public void PlaySound(SoundTypes s, Vector3 pos, int priority = 128)
        {
            float pitchMin = 1 - 0.08f;
            float pitchMax = 1 + 0.08f;
            PlaySound(s, pos, pitchMin, pitchMax, priority);

        }
        public void PlaySound(SoundTypes s, Vector3 pos, float pitchMin, float pitchMax = 1, int priority = 128)
        {
            Sound sound = GetClip(s);
            if (sound == null) return;

            PlaySound(sound, pos, pitchMin, pitchMax, priority);
        }

        /// <summary>
        /// Play givin sound.
        /// </summary>
        /// <param name="sound"></param>
        /// <param name="pos"></param>
        /// <param name="mValume"></param>
        /// <param name="priority"></param>
        public void PlaySound(Sound sound, Vector3 pos, float pitchMin, float pitchMax = 1, int priority = 128)
        {

            AudioPoolSource audioPoolSource;
            if (AudioSources.Count < 1)
            {
                audioPoolSource = Instantiate(sourceObject, transform).GetComponent<AudioPoolSource>();
            }
            else
            {
                audioPoolSource = AudioSources[0];
                AudioSources.Remove(audioPoolSource);
                audioPoolSource.gameObject.SetActive(true);
            }
            audioPoolSource.transform.position = pos;
            AudioClip soundClip = sound.GetClip();

            audioPoolSource.PlayClip(soundClip, sound.Valume, pitchMin, pitchMax, sound.AudioMixerGroup, priority);
        }

        private Sound GetClip(SoundTypes s)
        {
            Sound sound = Array.Find(sounds, x => x.SoundType == s);
            if (sound == null) { Debug.Log($"Sound not fount {s}"); return null; }
            else return sound;
        }
        #endregion

       

        #region Audio Settings
        
        #endregion
    }

    [System.Serializable]
    public class Sound
    {
        public SoundTypes SoundType;
        public float Valume;
        public AudioMixerGroup AudioMixerGroup;
        [Header("Random Choose")]
        public AudioClip[] AudioClips;

        public AudioClip GetClip()
        {
            if (AudioClips.Length > 1)
                return AudioClips[UnityEngine.Random.Range(0, AudioClips.Length)];
            else
                return AudioClips[0];
        }
    }
}
public enum SoundTypes
{
    None,
    ProjectileHit,
    BtnClick,
    TilePlace,
    TowerFire,
    GoldProduce
}
