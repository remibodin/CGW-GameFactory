using UnityEngine;

namespace Assets.Scripts.Gameplay
{
    public class Souvenir : MonoBehaviour
    {
        public ParticleSystem ParticleSystem;

        public void ActivateDots()
        {
            ParticleSystem.Play();
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                GetComponent<Animator>().SetBool("Activated", true);
            }
        }
    }
}