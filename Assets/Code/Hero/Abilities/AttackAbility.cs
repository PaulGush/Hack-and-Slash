using Code.Utils;
using UnityEngine;

//USING STRATEGY PATTERN

namespace Code.Hero.Abilities
{
    [CreateAssetMenu(fileName = "AttackAbility", menuName = "Abilities/AttackAbility")]
    public class AttackAbility : Ability
    {
        public Sprite Icon;
        public float Duration;
        public float Range;
        public float Damage;
        public float CooldownDuration;

        private Cooldown m_cooldown = new Cooldown();
        
        public override void ExecuteAbility(Transform origin)
        {
            if (m_cooldown.IsOnCooldown())
            {
                Debug.Log("Ability is on cooldown. " + m_cooldown.GetRemainingTime() + " remaining.");
                return;
            }
            
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