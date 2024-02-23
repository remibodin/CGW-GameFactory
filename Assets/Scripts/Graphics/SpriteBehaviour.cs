using UnityEngine;

using Cgw.Assets;

namespace Cgw.Graphics
{
    public class SpriteBehaviour : AssetBehaviour<SpriteAsset>
    {
        public SpriteAsset Sprite
        {
            get { return Asset; }
            set { Asset = value; }
        }

        private SpriteRenderer m_renderer;

        private void Awake()
        {
            m_renderer = gameObject.GetOrAddComponent<SpriteRenderer>();
            m_renderer.drawMode = SpriteDrawMode.Sliced;
        }

        protected override void AssetUpdated()
        {
            m_renderer.sprite = Sprite.Sprite;
            m_renderer.size = new Vector2(
                Sprite.Texture.width / 150,
                Sprite.Texture.height / 150);
        }
    }
}
