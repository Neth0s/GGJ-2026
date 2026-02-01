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
            musicAudioSource.volume = 0.5f;
            guardSource.PlayOneShot(guardSuspicious);
            StartCoroutine(waitForSound(guardSource));
        }
    }

    public void PlayGuardCatchSound()
    {
        if (!guardSource.isPlaying)
        {
            musicAudioSource.volume = 0.5f;
            guardSource.PlayOneShot(guardCatch);
            StartCoroutine(waitForSound(guardSource));
        }
    }

    public void PlayAccuseAndLoop()
    {
        musicAudioSource.volume = 0.1f;
        musicAccusedSource.PlayOneShot(accusedClip);
        StartCoroutine(waitForSound(musicAccusedSource));
    }

    private IEnumerator waitForSound(AudioSource audioSource)
    {
        //Wait Until Sound has finished playing
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        musicAudioSource.volume = 1f;
    }
}
