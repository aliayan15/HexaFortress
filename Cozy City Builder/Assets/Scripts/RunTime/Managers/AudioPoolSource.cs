
using Managers;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.VFX;

[RequireComponent(typeof(UnityEngine.AudioSource))]
public class AudioPoolSource:MonoBehaviour

{
    [SerializeField] private bool canSetDeactive = true;
    [SerializeField]
    protected UnityEngine.AudioSource audioSource;
    private WaitForSeconds _waitTime = new WaitForSeconds(1);

    private void Reset()
    {
        audioSource = GetComponent<UnityEngine.AudioSource>();
    }


    /// <summary>
    /// Play clip.
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="volume"></param>
    /// <param name="pitchVariance"> Between 0-0.5</param>
    public virtual void PlayClip(AudioClip clip, float volume, float pitchVariance, AudioMixerGroup audioMixerGroup, int priority = 128)
    {
        this.audioSource.Stop();
        this.audioSource.clip = clip;
        this.audioSource.outputAudioMixerGroup = audioMixerGroup;
        this.audioSource.pitch = 1f + Random.Range(-pitchVariance, pitchVariance);
        this.audioSource.volume = volume;
        this.audioSource.priority = priority;
        float timer = clip.length + 0.1f;
        this.audioSource.Play();

        if (!canSetDeactive) return;
        _waitTime = new WaitForSeconds(timer);
        StartCoroutine(SetActiveToFalse());
    }
    /// <summary>
    /// Playr clip with Pitch setting.
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="volume"></param>
    /// <param name="pitchmin"></param>
    /// <param name="pitchmax"></param>
    /// <param name="audioMixerGroup"></param>
    /// <param name="priority"></param>
    public virtual void PlayClip(AudioClip clip, float volume, float pitchmin, float pitchmax, AudioMixerGroup audioMixerGroup, int priority = 128)
    {
        this.audioSource.Stop();
        this.audioSource.clip = clip;
        this.audioSource.outputAudioMixerGroup = audioMixerGroup;
        this.audioSource.pitch = Random.Range(pitchmin, pitchmax);
        this.audioSource.volume = volume;
        this.audioSource.priority = priority;
        float timer = clip.length + 0.1f;

        this.audioSource.Play();
        if (!canSetDeactive) return;
        _waitTime = new WaitForSeconds(timer);
        StartCoroutine(SetActiveToFalse());
    }

    public void StopClip()
    {
        audioSource.Stop();
    }

    private IEnumerator SetActiveToFalse()
    {
        yield return _waitTime;
        AudioManager.Instance.AudioSources.Add(this);
        gameObject.SetActive(false);
    }
}
