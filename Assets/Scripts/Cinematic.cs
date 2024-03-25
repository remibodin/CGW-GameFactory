using System;

using UnityEngine;
using UnityEngine.Playables;

using Cgw.Graphics;

namespace Cgw
{
    [RequireComponent(typeof(PlayableDirector))]
    public class Cinematic : MonoBehaviour
    {
        private PlayableDirector m_director;
        private Action m_callback;

        private void Awake()
        {
            if (!TryGetComponent(out m_director))
            {
                Debug.LogError("Cinematic component need a PlayableDirector", this);
                enabled = false;
                return;
            }
        }

        public void Play(Action p_callback)
        {
            m_callback = p_callback;
            m_director.stopped += PlayableDirector_OnStopped;
            m_director.time = 0;
            m_director.Play();
            Fade.ForceValue(0);
        }

        private void PlayableDirector_OnStopped(PlayableDirector _)
        {
            m_director.stopped -= PlayableDirector_OnStopped;
            Fade.ForceValue(1);
            m_callback?.Invoke();
            m_callback = null;
        }
        
    }
}