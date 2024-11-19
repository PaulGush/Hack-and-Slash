using UnityEngine;

namespace Code.Hero.Abilities
{
    public abstract class AbilityStrategy : ScriptableObject
    {
        public float Cooldown;
        public abstract void ExecuteAbility(Transform origin);
    }
}