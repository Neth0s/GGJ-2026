using UnityEngine;

public class PigeonParticle : MonoBehaviour
{
    ParticleSystem ps;
    public AudioSource audioSource;
    public AudioClip pigeonsAudioClip;

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
        audioSource = gameObject.GetComponent<AudioSource>();

    }

    private void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.transform.parent.tag == "Player")
        {
            ps.Play();
            if(!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(pigeonsAudioClip, 1);
            }
        }
    }
}
