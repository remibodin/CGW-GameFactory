using UnityEngine;

using Cgw.Assets;
using YamlDotNet.Serialization;

namespace Cgw.Graphics
{
    public class SpriteAsset : Asset
    {
        [YamlIgnore]
        public Texture2D Texture;
        [YamlIgnore]
        public Sprite Sprite;
    }
}
