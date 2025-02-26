using UnityEngine;

namespace Assets.Scripts.Gameplay
{
    public class Souvenir : MonoBehaviour
    {
        public ParticleSystem ParticleSystem;

        public void ActivateDots()
        {
            Debug.Log("dots");
            ParticleSystem.Play();
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("triggered");
            if (collision.tag == "Player")
            {
                GetComponent<Animator>().SetBool("Activated", true);
            }
        }
    }
}