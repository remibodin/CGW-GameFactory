using Cgw.Gameplay;

public class ExitConditionAragnaIsHere : LevelExitCondition
{
    public override bool IsValid => SpiderController.Instance != null;
} 
