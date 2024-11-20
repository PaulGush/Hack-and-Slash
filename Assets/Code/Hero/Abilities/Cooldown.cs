using System;
using System.Collections;
using UnityEngine;

namespace Code.Hero.Abilities
{
    public static class Cooldown
    {
        private static bool m_isOnCooldown = false;
        
        private static float m_remainingTime = 0f;

        public static event Action<float> OnCooldownBegin;
        public static event Action<float> OnCooldownTimerTick;
        
        public static bool IsOnCooldown() => m_isOnCooldown;

        public static IEnumerator Begin(float duration)
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

        public static float GetRemainingTime() => m_remainingTime;
    }
}