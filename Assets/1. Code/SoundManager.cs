using System;
using CleanRoom.Utils;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    private const string FILE_NAME = "sound-settings.json";
    private const string SFX = "SFXVolume";
    private const string MAIN = "MainVolume";

    public static SoundManager Instance { get; private set; }

    [Header("Mixer"), SerializeField]
    
    private AudioMixer mixer;

    [Header("Audio Mixer Groups"), SerializeField]
    
    private AudioMixerGroup mainGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;

    [Header("Audio Sources"), SerializeField]
    
    private AudioSource sfxSource;

    [Header("Audio Clips"), SerializeField]
    
    private SoundEffect[] soundEffects;

    [Serializable]
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

    private void Start()
    {
        SaveSystem.Load(FILE_NAME, out string contents);
        if (string.IsNullOrEmpty(contents))
        {
            return;
        }

        JObject soundSettings = JObject.Parse(contents);
        float sfx = soundSettings[SFX]!.ToObject<float>();
        float main = soundSettings[MAIN]!.ToObject<float>();

        SetVolumeSFX(sfx);
        SetVolumeMain(main);
    }

    private void OnDestroy()
    {
        JObject soundSettings = new();
        mixer.GetFloat(SFX, out float sfxVolume);
        soundSettings.Add(SFX, sfxVolume);

        mixer.GetFloat(MAIN, out float mainVolume);
        soundSettings.Add(MAIN, mainVolume);

        SaveSystem.Save(FILE_NAME, soundSettings.ToString());
    }

    private void SetupAudioSources()
    {
        if (sfxSource != null && sfxGroup != null)
        {
            sfxSource.outputAudioMixerGroup = sfxGroup;
        }
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

    public float GetVolume(string key)
    {
        mixer.GetFloat(key, out float value);
        return value;
    }

    public void SetVolumeSFX(float value)
    {
        mixer.SetFloat(SFX, value);
    }

    public void SetVolumeMain(float value)
    {
        mixer.SetFloat(MAIN, value);
    }
}