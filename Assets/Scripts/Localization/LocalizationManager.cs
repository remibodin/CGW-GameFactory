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

        public void SetLangage(string p_langage)
        {
            int langageId = GetLangageId(p_langage);
            if (langageId == -1)
            {
                return;
            }
            SetLangageId(langageId);
        }

        public void SetLangageId(int p_id)
        {
            m_currentLangageId = p_id;
            onLangageUpdated?.Invoke();
        }

        public int GetLangageId(string p_langage)
        {
            var firstRow = m_csvAsset.Data[0];
            var cell_id = 0;
            foreach (var cell in firstRow)
            {
                if (cell.ToLower() == p_langage.ToLower())
                {
                    return cell_id;
                }
                cell_id++;
            }
            UnityEngine.Debug.LogWarning($"{p_langage} not found in localization file");
            return -1;
        }

        public string GetLangage(int p_langageId)
        {
            var firstRow = m_csvAsset.Data[0];
            return firstRow[p_langageId + 1].ToLower();
        }

        public int GetLangageId()
        {
            return m_currentLangageId;
        }

        public string GetLangage()
        {
            return GetLangage(GetLangageId());
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