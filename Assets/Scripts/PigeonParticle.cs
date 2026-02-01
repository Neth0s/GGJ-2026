using System.Collections;
using UnityEngine;

public class PigeonParticle : MonoBehaviour
{
    ParticleSystem ps;
    [SerializeField] GameObject[] pigeonSprites;
    float timeBeforeComeback = 10f;
    bool canFlyAway;
    public AudioSource audioSource;
    public AudioClip pigeonsAudioClip;

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
        canFlyAway = true;
    }

    private void OnTriggerEnter(Collider other)
    {
       if(canFlyAway && other.gameObject.transform.parent.tag == "Player")
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
            StartCoroutine(waitForSeconds(timeBeforeComeback));
       }
    }

    IEnumerator waitForSeconds(float seconds)
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
