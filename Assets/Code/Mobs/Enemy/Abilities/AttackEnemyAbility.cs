using Code.Utils;
using UnityEngine;

namespace Code.Mobs.Goblin.Abilities
{
    [CreateAssetMenu(fileName = "AttackGoblinAbility", menuName = "Abilities/Goblin/AttackAbility")]
    public class AttackEnemyAbility : EnemyAbility
    {
        public override bool ExecuteAbility(Transform origin)
        {
            throw new System.NotImplementedException();
        }

        public override void BeginCooldown(float amount)
        {
            throw new System.NotImplementedException();
        }

        public override Cooldown GetCooldown()
        {
            throw new System.NotImplementedException();
        }

        public override AnimationClip GetAnimation()
        {
            throw new System.NotImplementedException();
        }
    }
}