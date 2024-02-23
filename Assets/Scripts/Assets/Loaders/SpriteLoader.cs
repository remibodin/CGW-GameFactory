using System.IO;
using System.Collections.Generic;

using UnityEngine;

using Cgw.Graphics;

namespace Cgw.Assets.Loaders
{
    public class SpriteLoader : AssetFileLoader<SpriteAsset>
    {
        private string[] m_extentions = new string[] { "png", "jpg" };

        public override IEnumerable<string> Extentions => m_extentions;

        public override SpriteAsset LoadAsset(string p_metadataPath, string p_path, SpriteAsset p_data)
        {
            var bImage = File.ReadAllBytes(p_path);
            var texture = new Texture2D(2, 2);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.LoadImage(bImage);

            var sprite = Sprite.Create(
                texture,
                new Rect(0.0f, 0.0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f),
                100,
                0u,
                SpriteMeshType.FullRect);

            p_data.Texture = texture;
            p_data.Sprite = sprite;

            return p_data;
        }
    }
}
