using YamlDotNet.Serialization;

using Cgw.Assets;

namespace Cgw
{
    public class Configuration : Asset
    {
        [YamlMember(Alias = "localization")]
        public string LocalizationFileIdentifier;

        // StartMenu
        [YamlMember(Alias = "menu_logo")]
        public string MenuLogoIdentifier;
        [YamlMember(Alias = "menu_logo_lighting")]
        public string MenuLogoLightingIdentifier;
        [YamlMember(Alias = "menu_title")]
        public string MenuTitleIdentifier;
        [YamlMember(Alias = "menu_halo_fx")]
        public string MenuHaloFXIdentifier;
        [YamlMember(Alias = "menu_background_color")]
        public string MenuBackgroundColor = "#000000";
        [YamlMember(Alias = "menu_loop_sfx")]
        public string MenuLoopButtonSfxIdentifier;
        [YamlMember(Alias = "menu_select_button_sfx")]
        public string MenuSelectButtonSfxIdentifier;

        // Fade
        [YamlMember(Alias = "fade_color")]
        public string FadeColor = "#000000";
        [YamlMember(Alias = "fade_speed")]
        public float FadeSpeed = 3;
    }
}