using UnityEngine;

namespace Code.Hero.Abilities
{
    [CreateAssetMenu(fileName = "SlashAbility", menuName = "Abilities/SlashAbility")]
    public class SlashAbilityStrategy : AbilityStrategy
    {
        public float Duration;
        public float Range;
        public float Damage;
        public float Cooldown;

        public override void ExecuteAbility(Transform origin)
        {
            Debug.Log("Slash!");
        }
    }
}