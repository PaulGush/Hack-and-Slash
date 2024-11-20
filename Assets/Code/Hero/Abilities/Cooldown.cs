using System;
using System.Collections;
using UnityEngine;

namespace Code.Hero.Abilities
{
    public class Cooldown
    {
        private bool m_isOnCooldown = false;
        
        private float m_remainingTime = 0f;

        public event Action<float> OnCooldownBegin;
        public event Action<float> OnCooldownTimerTick;
        
        public bool IsOnCooldown() => m_isOnCooldown;

        public IEnumerator Begin(float duration)
        {
            OnCooldownBegin?.Invoke(duration);
            m_isOnCooldown = true;
            
            float timer = duration;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                m_remainingTime = timer;
                OnCooldownTimerTick?.Invoke(m_remainingTime);
                yield return null;
            }

            m_remainingTime = 0;
            m_isOnCooldown = false;
        }

        public float GetRemainingTime() => m_remainingTime;
    }
}