using YamlDotNet.Serialization;

using Cgw.Assets;

namespace Cgw
{
    public class Configuration : Asset
    {
        // StartMenu
        [YamlMember(Alias = "logo")]
        public string LogoIdentifier;
        [YamlMember(Alias = "background_color")]
        public string BackgroundColor = "000000";
        [YamlMember(Alias = "loop_sfx")]
        public string LoopButtonSfxIdentifier;
        [YamlMember(Alias = "select_button_sfx")]
        public string SelectButtonSfxIdentifier;
    }
}