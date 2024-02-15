using UnityEngine;

namespace Cgw.Assets
{
    public class AssetBehaviour<T> : MonoBehaviour
        where T : Asset
    {
        private T m_asset;

        public T Asset
        {
            get { return m_asset; }
            set
            {
                if (m_asset == value &&
                    value != null)
                {
                    return;
                }
                if (m_asset != null)
                {
                    m_asset.OnUpdated -= OnUpdated;
                }
                m_asset = value;
                m_asset.OnUpdated += OnUpdated;
                AssetUpdated();
            }
        }

        private void OnUpdated(Asset p_asset)
        {
            Asset = p_asset as T;
        }

        protected virtual void AssetUpdated() { }
    }
}
