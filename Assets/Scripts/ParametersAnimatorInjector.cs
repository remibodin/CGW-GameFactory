using System.Linq;
using UnityEngine;

namespace Cgw
{
    public class ParametersAnimatorInjector : MonoBehaviour
    {
        public string HorizontalSpeedNameParameter;
        public string VerticalSpeedNameParameter;
        public float MaxXMotionValue = 1;

        protected Animator m_animator;
        protected SpriteRenderer m_renderer;
        protected Vector3 m_lastFramePosition;
        protected Vector3 m_motion;

        protected float m_FallTime = 0.0f;

        protected virtual Vector3 Motion => m_motion;

        protected virtual void Awake()
        {
            m_animator = GetComponent<Animator>();
            m_renderer = GetComponent<SpriteRenderer>();
        }

        protected virtual void LateUpdate()
        {
            var currentPosition = transform.position;
            m_motion = (currentPosition - m_lastFramePosition) / Time.deltaTime;

            if (currentPosition.y < m_lastFramePosition.y)
            {
                m_FallTime += Time.deltaTime;
            }
            else
            {
                m_FallTime = 0.0f;
            }
            if (m_animator.parameters.FirstOrDefault(x => x.name == "FallTime") != null)
            {
                m_animator.SetFloat("FallTime", m_FallTime);
            }

            m_lastFramePosition = transform.position;

            var absXMotion = Mathf.Abs(Motion.x);
            var normalizeXMotionValue = absXMotion / MaxXMotionValue;
            m_animator.SetFloat(HorizontalSpeedNameParameter, normalizeXMotionValue);

            if (!string.IsNullOrEmpty(VerticalSpeedNameParameter))
            {
                m_animator.SetFloat(VerticalSpeedNameParameter, Motion.y);
            }

            if (Mathf.Abs(Motion.x) > 0.01f)
            {
                var flip = Mathf.Sign(Motion.x) < 0;
                if (m_renderer != null)
                {
                    m_renderer.flipX = flip;
                }
                else
                {
                    var scale = transform.localScale;
                    scale.x = flip ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
                    transform.localScale = scale;
                }
            }
        }
    }
}
