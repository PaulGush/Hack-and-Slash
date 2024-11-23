using Code.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Hero.Abilities
{
    public abstract class AbilityStrategy : ScriptableObject
    {
        public abstract void ExecuteAbility(Transform origin);

        public abstract void BeginCooldown(float amount);
        
        public abstract Sprite GetIcon();
        
        public abstract Cooldown GetCooldown();
    }
}