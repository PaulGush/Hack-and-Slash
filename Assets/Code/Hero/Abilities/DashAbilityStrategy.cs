using System;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Hero.Abilities
{
    [CreateAssetMenu(fileName = "DashAbility", menuName = "Abilities/DashAbility")]
    public class DashAbilityStrategy : AbilityStrategy
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
                Debug.Log("Ability is on cooldown. " + m_cooldown.GetRemainingTime() + " remaining.");
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