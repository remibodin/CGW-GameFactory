using UnityEngine;

namespace Cgw
{
    public class PlayerAnimator : ParametersAnimatorInjector
    {
        private Gameplay.Player m_player;

        protected override Vector3 Motion => new Vector3(m_player.Motion.x / Time.deltaTime, base.Motion.y, 0);

        protected override void Awake()
        {
            base.Awake();
            m_player = GetComponent<Gameplay.Player>();
        }

        public void Attack()
        {
            m_animator.SetTrigger("Attack");
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            m_animator.SetBool("OnGround", m_player.OnGround);
        }
    }
}

