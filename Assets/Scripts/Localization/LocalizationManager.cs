using System;

using Cgw.Assets;

namespace Cgw.Localization
{
    public class LocalizationManager
    {
        public static readonly LocalizationManager Instance = new LocalizationManager();

        public event Action onLangageUpdated;

        private CSVFileAsset m_csvAsset;
        private int m_currentLangageId = 1; // 0 is the index of keys col

        public int Count =>  m_csvAsset.Data[0].Count - 1;

        public void SetResourceIdentifier(string p_identifier)
        {
            if  (m_csvAsset != null)
            {
                m_csvAsset.OnUpdated -= OnAssetUpdated;
            }
            m_csvAsset = ResourcesManager.Get<CSVFileAsset>(p_identifier);
            if  (m_csvAsset != null)
            {
                m_csvAsset.OnUpdated += OnAssetUpdated;
            }
            onLangageUpdated?.Invoke();
        }

        public void SetLangageId(int p_id)
        {
            m_currentLangageId = p_id + 1;
            onLangageUpdated?.Invoke();
        }

        public string Get(string p_key) 
        {
            return Get(p_key, m_currentLangageId);
        }

        public string Get(string p_key, int p_colId)
        {
            foreach (var row in m_csvAsset.Data)
            {
                if (row[0] == p_key &&
                    p_colId < row.Count &&
                    !string.IsNullOrEmpty(row[p_colId]))
                {
                    return row[p_colId];
                }
            }
            return $"#{p_key}";
        }

        private void OnAssetUpdated(Asset p_asset)
        {
            m_csvAsset.OnUpdated -= OnAssetUpdated;
            m_csvAsset = p_asset as CSVFileAsset;
            m_csvAsset.OnUpdated += OnAssetUpdated;
            onLangageUpdated?.Invoke();
        }
    }
}