using System;
using Code.Hero.Abilities;
using Code.Input;
using DependencyInjection;
using UnityEngine;

namespace Code.Hero.Animation
{
    public class HeroAnimationController : MonoBehaviour
    {
        [Inject, SerializeField] private Hero m_hero;
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
            m_animator.SetFloat("Speed", PlayerInputs.Instance.MoveInput.magnitude, 0.1f, Time.deltaTime);
        }

        private void PlayAnimation(AnimationClip animationClip)
        {
            m_animator.Play(animationClip.name);
        }

        private void Hero_OnAbilityExecuted(Ability ability)
        {
            PlayAnimation(ability.GetAnimation());
        }
    }
}
