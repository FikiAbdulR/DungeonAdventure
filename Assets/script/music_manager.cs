using System.Collections;
using UnityEngine;

public class music_manager : MonoBehaviour
{
    public static music_manager Instance;

    [Header("Music Clips")]
    [SerializeField] private AudioClip explorationMusic;
    [SerializeField] private AudioClip battleMusic;

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float musicVolume = 0.7f;

    private AudioSource sourceA;
    private AudioSource sourceB;
    private AudioSource activeSource;
    private AudioSource inactiveSource;

    private Coroutine fadeCoroutine;
    private MusicState currentState = MusicState.None;

    private enum MusicState
    {
        None,
        Exploration,
        Battle
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SetupAudioSources()
    {
        sourceA = gameObject.AddComponent<AudioSource>();
        sourceB = gameObject.AddComponent<AudioSource>();

        sourceA.loop = true;
        sourceB.loop = true;
        sourceA.playOnAwake = false;
        sourceB.playOnAwake = false;
        sourceA.volume = 0f;
        sourceB.volume = 0f;

        activeSource = sourceA;
        inactiveSource = sourceB;
    }

    public void PlayExploration()
    {
        if (currentState == MusicState.Exploration) return;
        currentState = MusicState.Exploration;
        CrossfadeTo(explorationMusic);
    }

    public void PlayBattle()
    {
        if (currentState == MusicState.Battle) return;
        currentState = MusicState.Battle;
        CrossfadeTo(battleMusic);
    }

    private void CrossfadeTo(AudioClip newClip)
    {
        if (newClip == null) return;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(CrossfadeRoutine(newClip));
    }

    private IEnumerator CrossfadeRoutine(AudioClip newClip)
    {
        inactiveSource.clip = newClip;
        inactiveSource.volume = 0f;
        inactiveSource.Play();

        float startVolumeActive = activeSource.volume;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float ratio = t / fadeDuration;

            activeSource.volume = Mathf.Lerp(startVolumeActive, 0f, ratio);
            inactiveSource.volume = Mathf.Lerp(0f, musicVolume, ratio);

            yield return null;
        }

        activeSource.Stop();
        activeSource.volume = 0f;

        var temp = activeSource;
        activeSource = inactiveSource;
        inactiveSource = temp;
    }

    public void StopMusic()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        sourceA.Stop();
        sourceB.Stop();
        currentState = MusicState.None;
    }

    public void SetVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (activeSource.isPlaying)
            activeSource.volume = musicVolume;
    }
}