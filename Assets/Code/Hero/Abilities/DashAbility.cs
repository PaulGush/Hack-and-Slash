using System;
using Code.Utils;
using UnityEngine;
using UnityEngine.UI;

//USING STRATEGY PATTERN

namespace Code.Hero.Abilities
{
    [CreateAssetMenu(fileName = "DashAbility", menuName = "Abilities/DashAbility")]
    public class DashAbility : Ability
    {
        public Sprite Icon;
        public float Duration;
        public float Force;
        public float CooldownDuration;

        private Cooldown m_cooldown = new Cooldown();
        public static event Action<float, float> OnDash;

        public override void ExecuteAbility(Transform origin)
        {
            if (m_cooldown.IsOnCooldown())
            {
                return;
            }

            OnDash?.Invoke(Force, Duration);
            BeginCooldown(CooldownDuration);
        }

        public override void BeginCooldown(float amount)
        {
            MonoInstance.Instance.StartCoroutine(m_cooldown.Begin(amount));
        }

        public override Sprite GetIcon() => Icon;
        
        public override Cooldown GetCooldown() => m_cooldown;
    }
}