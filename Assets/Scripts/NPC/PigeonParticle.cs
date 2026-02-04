using System.Collections;
using UnityEngine;

public class PigeonParticle : MonoBehaviour
{
    [SerializeField] private float resetCooldown = 10f;

    [Header("References")]
    [SerializeField] GameObject[] pigeonSprites;
    [SerializeField] private AudioClip pigeonsAudioClip;

    private ParticleSystem ps;
    private AudioSource audioSource;
    bool canFlyAway;

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();

        canFlyAway = true;
    }

    private void OnTriggerEnter(Collider other)
    {
       if(canFlyAway && other.gameObject.CompareTag("Player"))
       {
            ps.Play();

            if(!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(pigeonsAudioClip, 1);
            }
            foreach (GameObject p in pigeonSprites)
            {
                p.SetActive(false);
            }

            canFlyAway = false;
            StartCoroutine(WaitForSeconds(resetCooldown));
       }
    }

    IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Resetup();
    }

    void Resetup()
    {
        foreach (GameObject p in pigeonSprites)
        {
            p.SetActive(true);
        }
        ps.gameObject.SetActive(false);
        ps.gameObject.SetActive(true);
        canFlyAway = true;
    }
}
