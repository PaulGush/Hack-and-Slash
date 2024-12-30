using System.Collections;
using Code.Gameplay;
using Code.Utils;
using UnityEditor.Timeline.Actions;
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
        public float AbilityExecutionDelay;
        
        private Cooldown m_cooldown = new Cooldown();
        private Transform m_origin;
        public override bool ExecuteAbility(Transform origin)
        {
            if (m_cooldown.IsOnCooldown())
            {
                return false;
            }
            
            MonoInstance.Instance.StartCoroutine(DealDamage(origin, AbilityExecutionDelay));
            BeginCooldown(CooldownDuration);
            return true;
        }

        private IEnumerator DealDamage(Transform origin, float duration)
        {
            float timer = duration;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }

            foreach (var i in Physics.OverlapCapsule(m_origin.position, (m_origin.position + (m_origin.forward * Range)), Range))
            {
                if (i.TryGetComponent<Damageable>(out var damageable))
                {
                    damageable.TakeDamage(Damage);
                }
            }
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