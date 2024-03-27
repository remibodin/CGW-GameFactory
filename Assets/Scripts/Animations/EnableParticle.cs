using UnityEngine;

namespace Cgw.Animations
{
    public class EnableParticle : StateMachineBehaviour
    {
        public string BoneName;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var root = animator.transform;

            var bone = root.Find(BoneName);
            if (bone == null)
            {
                Debug.LogWarning($"[EnableParticle] {BoneName} not found in {root.gameObject.name}");
                return;
            }

            var particle = bone.GetComponent<ParticleSystem>();
            if (particle == null)
            {
                Debug.LogError($"[EnableParticle] ParticleSystem not found in {root.gameObject.name}.{BoneName}");
                return;
            }

            particle.Play();
        }
    }
}
