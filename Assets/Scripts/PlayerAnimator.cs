using UnityEngine;

using Cgw.Assets;

public class PlayerAnimator : MonoBehaviour
{
    private Cgw.Gameplay.Player m_player;
    private Animator m_animator;
    private SpriteRenderer m_renderer;
    private Vector3 m_lastFramePosition;
    private Vector3 m_motion;

    void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_renderer = GetComponent<SpriteRenderer>();
        m_player = GetComponent<Cgw.Gameplay.Player>();
    }

    void LateUpdate()
    {
        var currentPosition = transform.position;
        m_motion = currentPosition - m_lastFramePosition;
        m_lastFramePosition = transform.position;

        m_animator.SetFloat("AbsH", Mathf.Abs(m_player.Motion.x) * 100f);
        m_animator.SetFloat("V", m_motion.y * 100f);
        m_animator.SetBool("OnGround", m_player.OnGround);

        if (m_renderer != null)
        {
            if (m_motion.x != 0)
            {
                m_renderer.flipX = m_player.Motion.x < 0;
            }
        }
        else
        {
            if (m_player.Motion.x != 0)
            {
                var scale = transform.localScale;
                scale.x = m_player.Motion.x < 0 ? - Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
                transform.localScale = scale;
            }
        }
    }
}
