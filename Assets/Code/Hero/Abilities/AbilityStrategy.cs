using UnityEngine;

namespace Code.Hero.Abilities
{
    public abstract class AbilityStrategy : ScriptableObject
    {
        public abstract void ExecuteAbility(Transform origin);
    }
}