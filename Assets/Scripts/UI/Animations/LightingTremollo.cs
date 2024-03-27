using UnityEngine;

using DG.Tweening;

namespace Cgw.UI.Animations
{
    public class LightingTremollo : MonoBehaviour
    {
        [SerializeField] private CanvasGroup m_group = default;
        [SerializeField] private RectTransform m_rt = default;
        [SerializeField] private float m_loopDuration = 1.0f;

        [Header("Light Intensity")]
        [SerializeField] private float m_minOpacity = 0.8f;
        [SerializeField] private float m_maxOpacity = 1.0f;
        [SerializeField] private int m_lightStepsPerLoop = 10;

        [Header("Light Movement")]
        [SerializeField] private float m_shakeStrength = 1.0f;
        [SerializeField] private int m_vibrato = 0;
        [SerializeField] private float m_randomness = 30.0f;

        private Sequence m_animSeq = null;

        private void Start()
        {
            PlayAnim();
        }

        private void OnDestroy()
        {
            if (!m_animSeq.IsActive())
                return;

            m_animSeq.Kill();
        }

        private void PlayAnim()
        {
            Fading();
            Shake();
        }

        private void Shake()
        {
            DOTween.Shake(
                () => m_rt.anchoredPosition3D, 
                value => m_rt.anchoredPosition3D = value, 
                m_loopDuration,
                m_shakeStrength,
                m_vibrato,
                m_randomness,
                true
            ).OnComplete(Shake);
        }

        private void Fading()
        {
            m_animSeq = DOTween.Sequence();

            float opacity;
            float duration = m_loopDuration * m_lightStepsPerLoop;
            for (int i = 0; i < m_lightStepsPerLoop; ++i)
            {
                opacity = Random.Range(m_minOpacity, m_maxOpacity);
                duration = Random.Range(duration * 0.2f, duration * 1.2f);
                m_animSeq.Append(m_group.DOFade(opacity, duration));
            }

            m_animSeq.OnComplete(Fading);
        }
    }
}