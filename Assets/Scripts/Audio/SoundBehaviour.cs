using UnityEngine;

using Cgw.Assets;
using System.Collections;

namespace Cgw.Audio
{
    public class SoundBehaviour : AssetBehaviour<SoundAsset>
    {
        private AudioSource m_source;

        private void Awake()
        {
            m_source = gameObject.GetOrAddComponent<AudioSource>();
        }

        protected override void AssetUpdated()
        {
            StartCoroutine(WaitLoading());
        }

        private IEnumerator WaitLoading()
        {
            yield return new WaitUntil(() => Asset.LoadingCoroutine == null);

            if (m_source.isPlaying)
            {
                m_source.Stop();
            }

            m_source.clip = Asset.AudioClip;
            m_source.volume = Asset.Volume;

            m_source.loop = true;
            m_source.Play();
        }
    }
}
