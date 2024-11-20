using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Hero.Abilities
{
    [CreateAssetMenu(fileName = "DashAbility", menuName = "Abilities/DashAbility")]
    public class DashAbilityStrategy : AbilityStrategy
    {
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
            MonoInstance.Instance.StartCoroutine(Cooldown.Begin(CooldownDuration));
        }
    }
}