using UnityEngine;
using Cgw.Localization;

public class SubtitleTigger : MonoBehaviour
{
    public string LocalizationKey;
    public float RepetitionDelay = 10f;
    private LocalizedString m_localizedString;
    private float _lastActivationTime = 0;

    private void Awake()
    {
        m_localizedString = new LocalizedString(LocalizationKey);
        _lastActivationTime = -RepetitionDelay;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.gameObject.CompareTag("Player"))
        {
            return;
        }

        if (Time.time < _lastActivationTime + RepetitionDelay)
        {
            return;
        }

        _lastActivationTime = Time.time;
        Subtitle.Display(m_localizedString.Value, 5f);
    }
}
