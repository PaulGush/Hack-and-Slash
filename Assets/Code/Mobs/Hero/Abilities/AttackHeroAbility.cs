using Code.Gameplay;
using Code.Utils;
using UnityEngine;

//USING STRATEGY PATTERN

namespace Code.Mobs.Hero.Abilities
{
    [CreateAssetMenu(fileName = "AttackHeroAbility", menuName = "Abilities/Hero/AttackAbility")]
    public class AttackHeroAbility : HeroAbility
    {
        public Sprite Icon;
        public float Duration;
        public float Range;
        public int Damage;
        public float CooldownDuration;
        public AnimationClip AnimationClip;
        
        private Cooldown m_cooldown = new Cooldown();
        
        public override bool ExecuteAbility(Transform origin)
        {
            if (m_cooldown.IsOnCooldown())
            {
                return false;
            }

            foreach (var i in Physics.OverlapCapsule(origin.position, (origin.position + (origin.forward * Range)), Range))
            {
                if (i.TryGetComponent<Damageable>(out Damageable damageable))
                {
                    damageable.TakeDamage(Damage);
                }
            }
            
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