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
        public static event Action<float, float> OnDash;

        public override void ExecuteAbility(Transform origin)
        {
            if (Cooldown.IsOnCooldown())
            {
                Debug.Log("Ability is on cooldown. " + Cooldown.GetRemainingTime() + " remaining.");
                return;
            }

            OnDash?.Invoke(Force, Duration);
            BeginCooldown(CooldownDuration);
        }

        public override void BeginCooldown(float amount)
        {
            MonoInstance.Instance.StartCoroutine(Cooldown.Begin(amount));
        }

        public override Sprite GetIcon() => Icon;
    }
}