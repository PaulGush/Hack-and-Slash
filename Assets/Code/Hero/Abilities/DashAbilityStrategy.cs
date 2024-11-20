using System.Collections;
using UnityEngine;

namespace Code.Hero.Abilities
{
    [CreateAssetMenu(fileName = "DashAbility", menuName = "Abilities/DashAbility")]
    public class DashAbilityStrategy : AbilityStrategy
    {
        public float Duration;
        public float Range;
        public float CooldownDuration;

        public override void ExecuteAbility(Transform origin)
        {
            if (Cooldown.IsOnCooldown())
            {
                Debug.Log("Ability is on cooldown. " + Cooldown.GetRemainingTime() + " remaining.");
                return;
            }

            MonoInstance.Instance.StartCoroutine(Dash(origin));
            MonoInstance.Instance.StartCoroutine(Cooldown.Begin(CooldownDuration));
        }

        IEnumerator Dash(Transform origin)
        {
            var targetPosition = origin.position;
            targetPosition += origin.forward * Range;
            
            float timer = 0f;
            while (timer < Duration)
            {
                timer += Time.deltaTime;
                origin.position = Vector3.Lerp(origin.position, targetPosition, timer / Duration);
                yield return null;
            }
            
            origin.position = targetPosition;
        }
    }
}