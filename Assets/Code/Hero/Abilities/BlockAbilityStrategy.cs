using Code.Utils;
using UnityEngine;

namespace Code.Hero.Abilities
{
    [CreateAssetMenu(fileName = "BlockAbility", menuName = "Abilities/Block Ability")]
    public class BlockAbilityStrategy : AbilityStrategy
    {
        public Sprite Icon;
        public float Duration;
        public float Range;
        public float MaxDamageAbsorption;
        public float CooldownDuration;
        
        private Cooldown m_cooldown = new Cooldown();
        
        public override void ExecuteAbility(Transform origin)
        {
            throw new System.NotImplementedException();
        }

        public override void BeginCooldown(float amount)
        {
            MonoInstance.Instance.StartCoroutine(m_cooldown.Begin(amount));        
        }

        public override Sprite GetIcon() => Icon;
        public override Cooldown GetCooldown() => m_cooldown;
    }
}