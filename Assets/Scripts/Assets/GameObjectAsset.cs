using UnityEngine;
using YamlDotNet.Serialization;

namespace Cgw.Assets
{
    public class GameObjectAsset : Asset 
    {
        [YamlMember(Alias = "asset_name")]
        public string AssetName;

        [YamlIgnore]
        public GameObject GameObject;
    }
}