using Code.Utils;
using UnityEngine;

//USING STRATEGY PATTERN

namespace Code.Hero.Abilities
{
    public abstract class Ability : ScriptableObject
    {
        public enum ExecuteType
        {
            toggle,
            single
        }

        public ExecuteType Type = ExecuteType.single;
        
        public abstract bool ExecuteAbility(Transform origin);

        public abstract void BeginCooldown(float amount);
        
        public abstract Sprite GetIcon();
        
        public abstract Cooldown GetCooldown();
        
        public abstract AnimationClip GetAnimation();
    }
}