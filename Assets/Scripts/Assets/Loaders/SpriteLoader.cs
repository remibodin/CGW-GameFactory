using System.IO;

using UnityEngine;

using Cgw.Graphics;

namespace Cgw.Assets.Loaders
{
    public class SpriteLoader : AssetLoader<SpriteAsset>
    {
        public override SpriteAsset LoadAsset(string p_path, SpriteAsset p_data)
        {
            var imagePath = Path.ChangeExtension(p_path, "png");
            var bImage = File.ReadAllBytes(imagePath);

            var texture = new Texture2D(2, 2);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.LoadImage(bImage);

            var sprite = UnityEngine.Sprite.Create(
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
