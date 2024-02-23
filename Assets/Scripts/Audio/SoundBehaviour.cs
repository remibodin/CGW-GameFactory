using UnityEngine;

using Cgw.Assets;
using System.Collections;

namespace Cgw.Audio
{
    public class SoundBehaviour : AssetBehaviour<SoundAsset>
    {
        public AudioSource Source { get; private set; }

        private void Awake()
        {
            Source = gameObject.GetOrAddComponent<AudioSource>();
        }

        protected override void AssetUpdated()
        {
            StartCoroutine(WaitLoading());
        }

        private IEnumerator WaitLoading()
        {
            yield return new WaitUntil(() => Asset.LoadingCoroutine == null);

            var isPlaying = Source.isPlaying;

            if (isPlaying)
            {
                Source.Stop();
            }

            Source.clip = Asset.AudioClip;
            Source.volume = Asset.Volume;
            Source.loop = Asset.Loop;

            if (isPlaying)
            {
                Source.Play();
            }
        }
    }
}
