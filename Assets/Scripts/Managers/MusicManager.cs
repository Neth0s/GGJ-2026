using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance = null;
    public static MusicManager Instance => instance;

    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource musicAccusedSource;
    [SerializeField] private AudioSource guardSource;
    [SerializeField] private AudioClip musicClip;
    [SerializeField] private AudioClip accusedClip;
    [SerializeField] private AudioClip guardSuspicious;
    [SerializeField] private AudioClip guardCatch;

    private float musicVolume = 1f;
    private float effectsVolume = 1f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        musicAudioSource.clip = musicClip;
        musicAudioSource.loop = true;
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicAudioSource.volume = musicVolume;
    }

    public void SetEffectsVolume(float volume)
    {
        effectsVolume = volume;
        guardSource.volume = effectsVolume;
        musicAccusedSource.volume = effectsVolume;
    }

    public void StopMusic()
    {
        musicAudioSource.Stop();
    }

    public void PlayMusic()
    {
        musicAudioSource.Play();
    }

    public void PlayGuardSuspiciousSound()
    {
        if (!guardSource.isPlaying)
        {
            musicAudioSource.volume = 0.5f * musicVolume;
            guardSource.PlayOneShot(guardSuspicious);
            StartCoroutine(WaitForSound(guardSource));
        }
    }

    public void PlayGuardCatchSound()
    {
        if (!guardSource.isPlaying)
        {
            musicAudioSource.volume = 0.5f * musicVolume;
            guardSource.PlayOneShot(guardCatch);
            StartCoroutine(WaitForSound(guardSource));
        }
    }

    public void PlayAccuseAndLoop()
    {
        musicAudioSource.volume = 0.1f * musicVolume;
        musicAccusedSource.PlayOneShot(accusedClip);
        StartCoroutine(WaitForSound(musicAccusedSource));
    }

    private IEnumerator WaitForSound(AudioSource audioSource)
    {
        //Wait Until Sound has finished playing
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        musicAudioSource.volume = 1f * musicVolume;
    }
}
