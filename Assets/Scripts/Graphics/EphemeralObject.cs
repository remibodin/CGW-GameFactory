using System.Collections;
using UnityEngine;

namespace Cgw
{
    public class EphemeralObject : MonoBehaviour
    {
        public float Lifetime = 3.0f;

        public void Start()
        {
            CoroutineRunner.StartCoroutine(EndOfLife());
        }

        public IEnumerator EndOfLife()
        {
            yield return new WaitForSeconds(Lifetime);
            Destroy(gameObject);
        }
    }
}
