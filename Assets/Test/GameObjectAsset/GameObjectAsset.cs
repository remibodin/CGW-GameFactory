
using UnityEngine;

using Cgw.Assets;
using Cgw.Assets.Loaders;

namespace Cgw.Test
{
    public class GameObjectAssetTest : MonoBehaviour
    {
        public string Identifier;

        private GameObjectAsset m_prefab;
        private GameObject m_gameObject;

        private void Start()
        {
            m_prefab = ResourcesManager.Get<GameObjectAsset>(Identifier);
            m_gameObject = Instantiate(m_prefab.GameObject, transform);
            m_prefab.OnUpdated += OnUpdated;
        }

        private void OnUpdated(Asset p_asset)
        {
            Destroy(m_gameObject);
            m_prefab.OnUpdated -= OnUpdated;
            m_prefab = p_asset as GameObjectAsset;
            m_gameObject = Instantiate(m_prefab.GameObject, transform);
            m_prefab.OnUpdated += OnUpdated;
        }

    }
}

