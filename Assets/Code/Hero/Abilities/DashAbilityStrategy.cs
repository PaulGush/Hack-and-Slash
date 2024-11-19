using System;
using System.Collections;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Code.Hero.Abilities
{
    [CreateAssetMenu(fileName = "DashAbility", menuName = "Abilities/DashAbility")]
    public class DashAbilityStrategy : AbilityStrategy
    {
        public float Duration;
        public float Range;
        
        public override void ExecuteAbility(Transform origin)
        {
            MonoInstance.Instance.StartCoroutine(Dash(origin));
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