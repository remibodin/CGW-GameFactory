using Cgw.Gameplay;
using Cgw.Localization;

public class ExitConditionAragnaIsHere : LevelExitCondition
{
    public override bool HasMessage => true;
    public override string Message => m_localizedString.Value;
    public override bool IsValid => SpiderController.Instance != null;

    public string LocalizationKey;
    private LocalizedString m_localizedString;

    private void Awake()
    {
        m_localizedString = new LocalizedString(LocalizationKey);
    }
} 
