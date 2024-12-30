using System;
using Code.Utils;
using UnityEngine;

//USING STRATEGY PATTERN

namespace Code.Mobs.Hero.Abilities
{
    [CreateAssetMenu(fileName = "DashHeroAbility", menuName = "Abilities/Hero/DashAbility")]
    public class DashHeroAbility : HeroAbility
    {
        public Sprite Icon;
        public float Duration;
        public float Force;
        public float CooldownDuration;
        public AnimationClip AnimationClip;
        
        private Cooldown m_cooldown = new Cooldown();
        public static event Action<float, float> OnDash;

        public override bool ExecuteAbility(Transform origin)
        {
            if (m_cooldown.IsOnCooldown())
            {
                return false;
            }

            OnDash?.Invoke(Force, Duration);
            BeginCooldown(CooldownDuration);
            return true;
        }

        public override void BeginCooldown(float amount)
        {
            MonoInstance.Instance.StartCoroutine(m_cooldown.Begin(amount));
        }

        public override Sprite GetIcon() => Icon;
        
        public override Cooldown GetCooldown() => m_cooldown;
        public override AnimationClip GetAnimation() => AnimationClip;
    }
}