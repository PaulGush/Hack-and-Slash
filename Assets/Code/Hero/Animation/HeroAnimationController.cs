using Code.Input;
using UnityEngine;

namespace Code.Hero.Animation
{
    public class HeroAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator m_animator;

        private void Update()
        {
            UpdateAnimParams();
        }

        private void UpdateAnimParams()
        {
            m_animator.SetFloat("Speed", PlayerInputs.Instance.MoveInput.magnitude, 0.1f, Time.deltaTime);
        }
    }
}
