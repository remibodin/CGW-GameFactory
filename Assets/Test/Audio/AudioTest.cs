using UnityEngine;

using Cgw.Assets;
using Cgw.Audio;

namespace Cgw.Test
{
    public class AudioTest : MonoBehaviour
    {
        public string Identifier;
        void Start()
        {
            var soundBehaviour = gameObject.AddComponent<SoundBehaviour>();
            soundBehaviour.Asset = ResourcesManager.Get<SoundAsset>(Identifier);
        }
    }
}
