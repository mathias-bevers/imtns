using System;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Mixer Groups")]
    [SerializeField] private AudioMixerGroup sfxGroup;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    [SerializeField] private SoundEffect[] soundEffects;

    [System.Serializable]
    public struct SoundEffect
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SetupAudioSources();
    }

    private void SetupAudioSources()
    {
        if (sfxSource != null && sfxGroup != null)
            sfxSource.outputAudioMixerGroup = sfxGroup;
    }
    public void PlaySFX(string soundName)
    {
        SoundEffect effect = Array.Find(soundEffects, x => x.name == soundName);

        if (effect.clip != null)
        {
            sfxSource.PlayOneShot(effect.clip, effect.volume);
        }
        else
        {
            Debug.LogWarning($"Sound Effect: {soundName} not found!");
        }
    }
}