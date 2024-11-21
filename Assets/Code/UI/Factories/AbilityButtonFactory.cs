using Code.Hero.Abilities;
using UnityEngine;

namespace Code.UI.Factories
{
    [CreateAssetMenu(fileName = "AbilityButtonFactory", menuName = "AbilityButtonFactory")]
    public sealed class AbilityButtonFactory : ScriptableObject
    {
        [SerializeField] private GameObject m_abilityButtonPrefab;
        public void CreateButton(AbilityStrategy strategy, Transform parentTransform)
        {
            AbilityButton abilityButton = new AbilityButton.Builder()
                .WithPrefab(m_abilityButtonPrefab)
                .WithAbility(strategy)
                .WithCooldown(strategy.GetCooldown())
                .WithParent(parentTransform)
                .Build();
        }
    }
}