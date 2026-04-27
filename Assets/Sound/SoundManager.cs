using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSourcePrefab;
    [SerializeField] private int sfxPoolSize = 10; // number of AudioSources for SFX

    [Header("Audio Clips")]
    [SerializeField] private List<AudioClip> musicClips = new List<AudioClip>();
    [SerializeField] private List<AudioClip> sfxClips = new List<AudioClip>();

    private Dictionary<string, AudioClip> musicDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> sfxDict = new Dictionary<string, AudioClip>();
    private List<AudioSource> sfxSources = new List<AudioSource>();
    private int currentSFXIndex = 0;

    [Header("Volume Settings")]
    [Range(0, 1)] public float musicVolume = 1f;
    [Range(0, 1)] public float sfxVolume = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            EnsureAudioSources();
            LoadClips();
            SetupSFXPool();
            SetMusicVolume(musicVolume);
            SetSFXVolume(sfxVolume);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void EnsureAudioSources()
    {
        if (musicSource == null)
        {
            musicSource = gameObject.GetComponent<AudioSource>();
            if (musicSource == null)
                musicSource = gameObject.AddComponent<AudioSource>();

            musicSource.playOnAwake = false;
            musicSource.loop = true;
        }

        if (sfxSourcePrefab == null)
        {
            GameObject template = new GameObject("SFXSourceTemplate");
            template.transform.SetParent(transform, false);

            sfxSourcePrefab = template.AddComponent<AudioSource>();
            sfxSourcePrefab.playOnAwake = false;
            sfxSourcePrefab.spatialBlend = 0f;
        }
    }

    private void LoadClips()
    {
        foreach (var clip in musicClips)
            if (!musicDict.ContainsKey(clip.name))
                musicDict.Add(clip.name, clip);

        foreach (var clip in sfxClips)
            if (!sfxDict.ContainsKey(clip.name))
                sfxDict.Add(clip.name, clip);
    }

    private void SetupSFXPool()
    {
        if (sfxPoolSize <= 0)
            sfxPoolSize = 1;

        for (int i = 0; i < sfxPoolSize; i++)
        {
            AudioSource source = Instantiate(sfxSourcePrefab, transform);
            source.playOnAwake = false;
            source.volume = sfxVolume;
            sfxSources.Add(source);
        }

        if (sfxSourcePrefab != null && sfxSourcePrefab.gameObject.name == "SFXSourceTemplate")
            sfxSourcePrefab.gameObject.SetActive(false);
    }

    // --- Music ---
    public void PlayMusic(string clipName, bool loop = true)
    {
        if (musicSource == null)
        {
            Debug.LogWarning("Music source is missing.");
            return;
        }

        if (musicDict.TryGetValue(clipName, out AudioClip clip))
        {
            if (musicSource.clip == clip && musicSource.isPlaying) return;
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.volume = musicVolume;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning($"Music clip '{clipName}' not found!");
        }
    }

    public void StopMusic() => musicSource.Stop();

    public void FadeMusic(float targetVolume, float duration)
    {
        StartCoroutine(FadeAudio(musicSource, targetVolume, duration));
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
    }

    // --- SFX ---
    public void PlaySFX(string clipName, float volume = 1f, float pitch = 1f)
    {
        if (sfxSources.Count == 0)
            SetupSFXPool();

        if (sfxSources.Count == 0)
        {
            Debug.LogWarning("No SFX sources available.");
            return;
        }

        if (sfxDict.TryGetValue(clipName, out AudioClip clip))
        {
            AudioSource source = sfxSources[currentSFXIndex];
            currentSFXIndex = (currentSFXIndex + 1) % sfxSources.Count;

            source.clip = clip;
            source.volume = Mathf.Clamp01(volume * sfxVolume);
            source.pitch = pitch;
            source.Play();
        }
        else
        {
            Debug.LogWarning($"SFX clip '{clipName}' not found!");
        }
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        foreach (var source in sfxSources)
            source.volume = sfxVolume;
    }

    // --- Helper ---
    private System.Collections.IEnumerator FadeAudio(AudioSource source, float targetVolume, float duration)
    {
        float startVolume = source.volume;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, targetVolume, time / duration);
            yield return null;
        }

        source.volume = targetVolume;
    }
}
