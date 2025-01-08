using Code.Gameplay;
using Code.Input;
using Code.Mobs.Enemy.Abilities;
using UnityEngine;
using UnityEngine.AI;

namespace Code.Mobs.Enemy.Animation
{
    public class EnemyAnimationController : MonoBehaviour
    {
        private static readonly int Speed = Animator.StringToHash("Speed");
        [Header("References")]
        [SerializeField] private Enemy m_enemy;
        [SerializeField] private Animator m_animator;
        [SerializeField] private HealthSystem m_healthSystem;
        [SerializeField] private NavMeshAgent m_navMeshAgent;

        [Space(10)] 
        [SerializeField] private AnimationClip m_onDamaged;
        [SerializeField] private AnimationClip m_onDeath;
        
        private void OnEnable()
        {
            m_enemy.OnAbilityExecuted += Enemy_OnAbilityExecuted;
            m_healthSystem.OnDamaged += HealthSystem_OnDamaged;
            m_healthSystem.OnDeath += HealthSystem_OnDeath;
        }

        private void OnDisable()
        {
            m_enemy.OnAbilityExecuted -= Enemy_OnAbilityExecuted;
            m_healthSystem.OnDamaged -= HealthSystem_OnDamaged;
            m_healthSystem.OnDeath -= HealthSystem_OnDeath;
        }

        private void Update()
        {
            UpdateAnimParams();
        }

        private void UpdateAnimParams()
        {
            m_animator.SetFloat(Speed, m_navMeshAgent.velocity.magnitude, 0.1f, Time.deltaTime);
        }

        public void PlayAnimation(AnimationClip animationClip)
        {
            m_animator.Play(animationClip.name);
        }

        private void Enemy_OnAbilityExecuted(EnemyAbility enemyAbility)
        {
            PlayAnimation(enemyAbility.GetAnimation());
        }

        private void HealthSystem_OnDamaged()
        {
            PlayAnimation(m_onDamaged);
        }

        private void HealthSystem_OnDeath()
        {
            m_healthSystem.OnDamaged -= HealthSystem_OnDamaged;
            m_healthSystem.OnDeath -= HealthSystem_OnDeath;
            PlayAnimation(m_onDeath);
        }
    }
}