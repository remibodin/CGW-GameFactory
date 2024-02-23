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
        public float Volume = 1;

        [YamlMember(Alias = "pitch")]
        public float Pitch = 1;

        [YamlMember(Alias = "loop")]
        public bool Loop = false;

        [YamlMember(Alias = "auto_start")]
        public bool AutoStart = false;
    }
}
