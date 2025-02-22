using System.Linq;

using UnityEngine;

using Cgw.Assets;

public class LevelExit : MonoBehaviour
{
    [SerializeField] private string _nextSceneName;

    private LevelExitCondition[] _conditions;

    private void Awake()
    {
        _conditions = GetComponents<LevelExitCondition>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.gameObject.CompareTag("Player"))
        {
            return;
        }

        var notValidatedCondition = _conditions.FirstOrDefault((c) => !c.IsValid);
        if (notValidatedCondition != default)
        {
            if (notValidatedCondition.HasMessage)
            {
                Subtitle.Display(notValidatedCondition.Message, 5f);
            }
            return;
        }

        SceneLoader.Custom(_nextSceneName);

    }
}
