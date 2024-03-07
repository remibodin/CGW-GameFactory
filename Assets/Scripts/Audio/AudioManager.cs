using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Cgw.Assets;
using Cgw.Scripting;

namespace Cgw.Audio
{
    public class AudioManager
    {
        public static readonly AudioManager Instance = new AudioManager();

        private Queue<AudioSource> m_disabledSources = new ();
        private GameObject m_root;

        public void Init()
        {
            if (m_root != null)
            {
                return;
            }
            m_root = new GameObject("AudioManager");
            GameObject.DontDestroyOnLoad(m_root);
            for (int i= 0; i < 10; i++)
            {
                var source = Instance.CreateNewSource();
                m_disabledSources.Enqueue(source);
            }
            LuaEnvironment.AddEnvItem("AudioManager", this);
        }

        private AudioSource CreateNewSource()
        {
            var obj = new GameObject("AudioSource");
            obj.SetActive(false);
            obj.transform.parent = m_root.transform;
            return obj.AddComponent<AudioSource>();
        }

        public AudioSource Play(string p_identifier)
        {
            AudioSource source = null;
            if (Instance.m_disabledSources.Count <= 0)
            {
                source = Instance.CreateNewSource();
            }
            else
            {
                source = Instance.m_disabledSources.Dequeue();
            }
            source.gameObject.SetActive(true);
            var soundBehaviour = source.gameObject.GetOrAddComponent<SoundBehaviour>();
            soundBehaviour.Asset = ResourcesManager.Get<SoundAsset>(p_identifier);

            if (!soundBehaviour.Asset.Loop)
            {
                CoroutineRunner.StartCoroutine(AutoRelease(source));
            }

            return source;
        }

        private IEnumerator AutoRelease(AudioSource p_source)
        {
            yield return new WaitUntil(() => p_source.clip == null);
            yield return new WaitForSeconds(p_source.clip.length);
            Release(p_source);
        }

        public void Release(AudioSource p_source)
        {
            p_source.Stop();
            p_source.clip = null;

            var soundBehaviour = p_source.gameObject.GetComponent<SoundBehaviour>();
            GameObject.Destroy(soundBehaviour);

            p_source.gameObject.SetActive(false);
            m_disabledSources.Enqueue(p_source);
        }
    }
}