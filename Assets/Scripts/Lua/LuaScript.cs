using YamlDotNet.Serialization;

using Cgw.Assets;

namespace Cgw.Scripting
{
    public class LuaScript : Asset
    {
        [YamlIgnore]
        public string SourcePath;

        public LuaInstance CreateInstance()
        {
            var instance = new LuaInstance();
            instance.InitFromFile(SourcePath);
            return instance;
        }
    }
}