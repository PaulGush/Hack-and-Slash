using Code.Input;
using Code.Mobs.Hero.Abilities;
using DependencyInjection;
using UnityEngine;

namespace Code.Mobs.Hero.Animation
{
    public class HeroAnimationController : MonoBehaviour
    {
        private static readonly int Speed = Animator.StringToHash("Speed");
        [Inject, SerializeField] private Mobs.Hero.Hero m_hero;
        [SerializeField] private Animator m_animator;

        private void OnEnable()
        {
            m_hero.OnAbilityExecuted += Hero_OnAbilityExecuted;
        }

        private void OnDisable()
        {
            m_hero.OnAbilityExecuted -= Hero_OnAbilityExecuted;
        }

        private void Update()
        {
            UpdateAnimParams();
        }

        private void UpdateAnimParams()
        {
            m_animator.SetFloat(Speed, PlayerInputs.Instance.MoveInput.magnitude, 0.1f, Time.deltaTime);
        }

        private void PlayAnimation(AnimationClip animationClip)
        {
            m_animator.Play(animationClip.name);
        }

        private void Hero_OnAbilityExecuted(HeroAbility heroAbility)
        {
            PlayAnimation(heroAbility.GetAnimation());
        }
    }
}
