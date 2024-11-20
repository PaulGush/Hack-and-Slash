using UnityEngine;
using UnityEngine.UI;

namespace Code.Hero.Abilities
{
    [CreateAssetMenu(fileName = "SlashAbility", menuName = "Abilities/SlashAbility")]
    public class SlashAbilityStrategy : AbilityStrategy
    {
        public Sprite Icon;
        public float Duration;
        public float Range;
        public float Damage;
        public float CooldownDuration;

        public override void ExecuteAbility(Transform origin)
        {
            if (Cooldown.IsOnCooldown())
            {
                Debug.Log("Ability is on cooldown. " + Cooldown.GetRemainingTime() + " remaining.");
                return;
            }
            
            Debug.Log("Slash!");
            BeginCooldown(CooldownDuration);
        }

        public override void BeginCooldown(float amount)
        {
            MonoInstance.Instance.StartCoroutine(Cooldown.Begin(amount));        }

        public override Sprite GetIcon() => Icon;
    }
}