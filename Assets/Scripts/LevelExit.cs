using System.Linq;

using UnityEngine;

using Cgw.Assets;
using System.Collections;
using Cgw;
using Cgw.Gameplay;
using Cgw.Graphics;

public class LevelExit : MonoBehaviour
{
    [SerializeField] private string _nextSceneName;
    [SerializeField] private string _cinematicName;
    [SerializeField] private float _delay;
    [SerializeField] private bool _fadeBeforeCinematic = false;

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

        CoroutineRunner.StartCoroutine(CO_SceneLoader());
    }

    private IEnumerator CO_SceneLoader()
    {
        Player.Instance.enabled = false;
        if (_delay > 0)
        {
            yield return new WaitForSeconds(_delay);
        }
        if (string.IsNullOrEmpty(_cinematicName))
        {
            SceneLoader.Custom(_nextSceneName);
        }
        else
        {
            if (_fadeBeforeCinematic)
            {
                Fade.Out(() => SceneLoader.PlayCinematicAndLoadScene(_cinematicName, _nextSceneName));
            }
            else
            {
                SceneLoader.PlayCinematicAndLoadScene(_cinematicName, _nextSceneName);
            }
        }
    }
}
