using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cgw.Audio
{
    public class Sounds : MonoBehaviour
    {
        public string Identifier;

        private AudioSource m_source;

        private void OnEnable()
        {
            m_source = AudioManager.Instance.Play(Identifier);
        }

        private void OnDisable()
        {
            AudioManager.Instance.Release(m_source);
        }
    }
}
