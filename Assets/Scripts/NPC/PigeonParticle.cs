using System.Collections;
using UnityEngine;

public class PigeonParticle : MonoBehaviour
{
    [SerializeField] private float resetCooldown = 10f;

    [Header("References")]
    [SerializeField] private GameObject[] idlepigeonSprites;
    [SerializeField] private AudioClip pigeonsAudioClip;
    [SerializeField] private ParticleSystem psForward;
    [SerializeField] private ParticleSystem psBackward;

    private AudioSource audioSource;
    private bool canFlyAway;
    private bool waitForComeback;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        canFlyAway = true;
    }

    private void OnTriggerEnter(Collider other)
    {
       if(canFlyAway && other.gameObject.CompareTag("Player"))
       {
            psForward.Play();

            if(!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(pigeonsAudioClip, 1);
            }
            foreach (GameObject p in idlepigeonSprites)
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
        LaunchBackwardParticles();
    }

    private void Update()
    {
        if(waitForComeback && !psBackward.IsAlive())
        {
            Resetup();
            waitForComeback = false;
            psBackward.Clear();
        }
    }

    private void LaunchBackwardParticles()
    {
        psBackward.Play();
        waitForComeback = true;
    }


    private void Resetup()
    {

        foreach (GameObject p in idlepigeonSprites)
        {
            p.SetActive(true);
        }
        psForward.Clear();
        canFlyAway = true;
    }
}
