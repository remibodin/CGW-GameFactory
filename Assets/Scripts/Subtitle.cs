using System.Collections;
using Cgw;
using UnityEngine;

public class Subtitle : SingleBehaviourInScene<Subtitle>
{
    public static void Display(string text, float _time = 1f)
    {
        Instance._label.text = text;
        Instance._group.alpha = 1;
        CoroutineRunner.StartCoroutine(Instance.CO_Hide(_time));
    }

    [Header("REF")]
    [SerializeField] private CanvasGroup _group;
    [SerializeField] private TMPro.TMP_Text _label;

    private void Start()
    {
        _group.alpha = 0;
    }

    private IEnumerator CO_Hide(float delay)
    {
        yield return new WaitForSeconds(delay);
        _group.alpha = 0;
    }
}
