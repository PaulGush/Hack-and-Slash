using System;
using UnityEngine;

namespace Code.Gameplay
{
    public class HealthSystem : Damageable
    {
        public override event Action OnDamaged;
        public override event Action OnDeath;
        
        [SerializeField] private int m_currentHealth;
        [SerializeField] private int m_maxHealth = 100;

        private void Start()
        {
            SetCurrentHealth(m_maxHealth);
        }

        public override int TakeDamage(int damage)
        {
            OnDamaged?.Invoke();
            m_currentHealth -= damage;
            
            if (m_currentHealth <= 0)
            {
                OnDeath?.Invoke();
                return 0;
            }
            
            return m_currentHealth;
        }

        private void SetCurrentHealth(int health)
        {
            m_currentHealth = health;
        }
        
        public int GetCurrentHealth() => m_currentHealth;
    }
}
