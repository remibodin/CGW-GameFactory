using System.Collections.Generic;

namespace Cgw.Assets
{
    public class CSVFileAsset : Asset
    {
        public readonly List<List<string>> Data;

        public CSVFileAsset(List<List<string>> p_data)
        {
            Data = p_data;
        }
    }
}