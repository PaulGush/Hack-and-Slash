using System;
using Code.Gameplay;
using Code.Mobs.Enemy.Abilities;
using UnityEngine;

namespace Code.Mobs.Enemy
{
    public class Enemy : MonoBehaviour, IEntity
    {
        public event Action<EnemyAbility> OnAbilityExecuted;
        
        [SerializeField] private AttackEnemyAbility m_attackEnemyAbility;

        private void ExecuteAbility(EnemyAbility enemyAbility)
        {
            if (enemyAbility.Type == EnemyAbility.ExecuteType.single)
            {
                if (enemyAbility.ExecuteAbility(transform)) OnAbilityExecuted?.Invoke(enemyAbility);
            }
            else
            {
                enemyAbility.ExecuteAbility(transform); 
                OnAbilityExecuted?.Invoke(enemyAbility);
            }
        }

        public AnimationClip GetAbilityAnimationClip() => m_attackEnemyAbility.GetAnimation();
    }
}