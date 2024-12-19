using Code.Utils;
using UnityEngine;
using UnityEngine.Pool;

//USING STRATEGY PATTERN

namespace Code.Hero.Abilities
{
    [CreateAssetMenu(fileName = "SpecialAbility", menuName = "Abilities/SpecialAbility")]
    public class SpecialAbility : Ability
    {
        public Sprite Icon;
        public float Duration;
        public float Range;
        public float Damage;
        public float CooldownDuration;
        public AnimationClip AnimationClip;
        public GameObject Prefab;
        
        private Cooldown m_cooldown = new Cooldown();
        
        public override bool ExecuteAbility(Transform origin)
        {
            if (m_cooldown.IsOnCooldown())
            {
                return false;
            }

            BeginCooldown(CooldownDuration);
            Instantiate(Prefab, origin.position + (origin.forward * 2), Quaternion.identity);
            
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