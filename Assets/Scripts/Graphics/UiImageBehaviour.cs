using UnityEngine.UI;

using Cgw.Assets;

namespace Cgw.Graphics
{
    public class UiImageBehaviour : AssetBehaviour<SpriteAsset>
    {
        private Image m_renderer;

        private void Awake()
        {
            m_renderer = gameObject.GetOrAddComponent<Image>();
        }

        protected override void AssetUpdated()
        {
            m_renderer.sprite = Asset.Sprite;
        }
    }
}
