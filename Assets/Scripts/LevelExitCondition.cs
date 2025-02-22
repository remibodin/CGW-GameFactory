using UnityEngine;

public abstract class LevelExitCondition : MonoBehaviour
{
    public abstract bool IsValid { get; }
    public abstract bool HasMessage { get; }
    public abstract string Message { get; }
}
