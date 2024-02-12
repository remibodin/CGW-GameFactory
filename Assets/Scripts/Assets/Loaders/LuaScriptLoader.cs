using Cgw.Scripting;

namespace Cgw.Assets.Loaders
{
    public class LuaScriptLoader : AssetLoader<LuaScript>
    {
        public override LuaScript LoadAsset(string p_path, LuaScript p_data)
        {
            if (p_data == null)
            {
                p_data = new LuaScript();
            }
            p_data.SourcePath = System.IO.Path.ChangeExtension(p_path, "lua");
            return p_data;
        }
    }
}