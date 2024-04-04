using System.IO;

namespace Cgw.Assets.Loaders
{
    public class CSVFileLoader : IAssetLoader
    {
        public Asset Load(string p_path)
        {
            var filePath = Path.ChangeExtension(p_path, "csv");
            var data = yutokun.CSVParser.LoadFromPath(filePath);
            return new CSVFileAsset(data);
        }
    }
}