using System.Collections;
using Code.Utils;
using UnityEngine;

//USING STRATEGY PATTERN

namespace Code.Mobs.Hero.Abilities
{
    [CreateAssetMenu(fileName = "SpecialHeroAbility", menuName = "Abilities/Hero/SpecialAbility")]
    public class SpecialHeroAbility : HeroAbility
    {
        public Sprite Icon;
        public float Duration;
        public float Range;
        public float Damage;
        public float CooldownDuration;
        public AnimationClip AnimationClip;
        public GameObject Prefab;
        public float AbilityExecutionDelay;
        
        private Cooldown m_cooldown = new Cooldown();
        
        public override bool ExecuteAbility(Transform origin)
        {
            if (m_cooldown.IsOnCooldown())
            {
                return false;
            }

            MonoInstance.Instance.StartCoroutine(SpawnPrefab(origin, AbilityExecutionDelay));
            BeginCooldown(CooldownDuration);
            return true;
        }
        
        private IEnumerator SpawnPrefab(Transform origin, float duration)
        {
            float timer = duration;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
            
            Instantiate(Prefab, origin.position + (origin.forward * 2), Quaternion.identity);
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