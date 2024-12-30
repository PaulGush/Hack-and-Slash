using Code.Gameplay;
using Code.Utils;
using UnityEngine;

namespace Code.Mobs.Enemy.Abilities
{
    [CreateAssetMenu(fileName = "AttackGoblinAbility", menuName = "Abilities/Goblin/AttackAbility")]
    public class AttackEnemyAbility : EnemyAbility
    {
        [SerializeField] private AnimationClip m_executeAbilityAnimation;
        public float Range;
        public int Damage;
        public float CooldownDuration;
        
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

        public override Cooldown GetCooldown() => m_cooldown;

        public override AnimationClip GetAnimation() => m_executeAbilityAnimation;
        
    }
}