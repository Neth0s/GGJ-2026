using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance = null;
    public static MusicManager Instance => instance;

    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource musicAccusedSource;
    [SerializeField] private AudioClip musicClip;
    [SerializeField] private AudioClip accusedClip;

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


    public void PlayAccuseAndLoop()
    {
        musicAudioSource.volume = 0.1f;
        musicAccusedSource.PlayOneShot(accusedClip);
        StartCoroutine(waitForSound());
    }

    private IEnumerator waitForSound()
    {
        //Wait Until Sound has finished playing
        while (musicAccusedSource.isPlaying)
        {
            yield return null;
        }

        musicAudioSource.volume = 1f;
    }
}
