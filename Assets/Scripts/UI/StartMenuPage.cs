using System.Collections.Generic;

using UnityEngine;

using DG.Tweening;

namespace Cgw.UI
{
    public class StartMenuPage : MonoBehaviour
    {
        [SerializeField] private CanvasGroup m_group;
        [SerializeField] private List<CanvasGroup> m_elementsGroup;
        [SerializeField] private GameObject m_firstSelectedControl;

        [Header("Animations")]
        [SerializeField] private float m_showAnimDurationPerElement = 0.3f;
        [SerializeField] private float m_showAnimDelayBetweenElements = 0.1f; 
        [SerializeField] private Ease m_showAnimEasing = Ease.Linear;
        [SerializeField] private float m_hideAnimDuration = 0.15f;
        [SerializeField] private Ease m_hideAnimEasing = Ease.Linear;
        private Sequence m_animSeq = null;

        private void Awake()
        {
            // Preparing initial visibility for anim
            foreach (var element in m_elementsGroup)
            {
                element.alpha = 0.0f;
            }

            ForceHide();
        }

        /// <summary>
        /// Shows page with anims
        /// </summary>
        /// <returns>Total Duration of the animation</returns>
        public float Show(float p_delay = 0.0f)
        {
            if (m_elementsGroup.Count > 0)
            {
                // Show element one by one
                DisplayPage(true);
                return ShowElementsAnim(p_delay);
            }
            else
            {
                // Show group in one go
                return ShowGroupAnim(p_delay);
            }
        }

        /// <summary>
        /// Hides page with anims
        /// </summary>
        /// <returns>Total duration of the animation</returns>
        public float Hide(float p_delay = 0.0f)
        {
            return HideGroupAnim(p_delay);
        }

        /// <summary>
        /// Instantly shows page, bypassing anims
        /// </summary>
        public void ForceShow()
        {
            DisplayPage(true);
            DisplayPageElements(true);
        }

        /// <summary>
        /// Instantly hides page, bypassing anims
        /// </summary>
        public void ForceHide()
        {
            DisplayPage(false);
            DisplayPageElements(false);
        }

        private void DisplayPage(bool p_shown)
        {
            m_group.alpha = p_shown ? 1.0f : 0.0f;
            m_group.interactable = p_shown;
            m_group.blocksRaycasts = p_shown;
        }

        private void DisplayPageElements(bool p_shown)
        {
            float alphaValue = p_shown ? 1.0f : 0.0f;
            foreach (var element in m_elementsGroup)
            {
                element.alpha = alphaValue;
            }
        }

        private float ShowElementsAnim(float p_delay)
        {
            ResetAnimSequence();
            m_animSeq = DOTween.Sequence();
            float totalDuration = 0.0f;
            
            for (int i = 0; i < m_elementsGroup.Count; ++i)
            {
                float insertTime = i * m_showAnimDelayBetweenElements;
                m_animSeq.Insert(
                    insertTime, 
                    m_elementsGroup[i].DOFade(1.0f, m_showAnimDurationPerElement)
                        .SetEase(m_showAnimEasing)
                );
                totalDuration += m_showAnimDurationPerElement - insertTime;
            }

            if (p_delay > 0.0f)
                m_animSeq.PrependInterval(p_delay); // Prepend to avoid doing math for each insertTime

            m_animSeq = null;

            return totalDuration;
        }

        private float ShowGroupAnim(float p_delay)
        {
            ResetAnimSequence();
            m_animSeq = DOTween.Sequence();

            if (p_delay > 0.0f)
                m_animSeq.AppendInterval(p_delay);

            m_animSeq.Append(m_group.DOFade(1.0f, m_showAnimDurationPerElement)).SetEase(m_showAnimEasing);
            m_animSeq.OnComplete(() => ForceShow());

            m_animSeq = null;

            return m_showAnimDurationPerElement;
        }

        private float HideGroupAnim(float p_delay)
        {
            ResetAnimSequence();
            m_animSeq = DOTween.Sequence();

            if (p_delay > 0.0f)
                m_animSeq.AppendInterval(p_delay);

            m_animSeq.Append(m_group.DOFade(0.0f, m_hideAnimDuration)).SetEase(m_hideAnimEasing);
            m_animSeq.OnComplete(() => ForceHide());

            m_animSeq = null;

            return m_hideAnimDuration;
        }

        private void ResetAnimSequence()
        {
            if (m_animSeq == null)
                return;
            
            if (m_animSeq.IsPlaying())
                m_animSeq.Kill(true);
            
            m_animSeq = null;
        }
    }
}