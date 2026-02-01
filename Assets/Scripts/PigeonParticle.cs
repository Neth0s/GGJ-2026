using UnityEngine;

public class PigeonParticle : MonoBehaviour
{
    ParticleSystem ps;

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.transform.parent.tag == "Player")
        {
            ps.Play();
        }
    }
}
