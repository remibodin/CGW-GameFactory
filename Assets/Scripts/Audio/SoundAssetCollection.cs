using System.Collections.Generic;

using UnityEngine;

using Cgw.Assets;

using YamlDotNet.Serialization;

namespace Cgw.Audio
{
    public class SoundAssetCollection : Asset
    {
        [YamlMember(Alias = "sounds")]
        public List<string> SoundsIdentifier;
    }
}
