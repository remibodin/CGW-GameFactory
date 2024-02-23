using UnityEngine;

using Cgw.Assets;

using YamlDotNet.Serialization;

namespace Cgw.Audio
{
    public class SoundAsset : Asset
    {
        [YamlIgnore]
        public Coroutine LoadingCoroutine;
        [YamlIgnore]
        public AudioClip AudioClip;

        [YamlMember(Alias = "volume")]
        public float Volume;
    }
}
