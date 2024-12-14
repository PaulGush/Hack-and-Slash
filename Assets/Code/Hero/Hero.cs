using System.Collections.Generic;
using Code.Hero.Abilities;
using Code.UI;
using DependencyInjection;
using UnityEngine;

namespace Code.Hero
{
    public class Hero : MonoBehaviour, IDependencyProvider
    {
        [SerializeField] private List<AbilityStrategy> m_abilities;
        private void OnEnable()
        {
            AbilityButton.OnButtonPressed += ExecuteAbility;
        }

        private void OnDisable()
        {
            AbilityButton.OnButtonPressed -= ExecuteAbility;
        }

        void ExecuteAbility(AbilityStrategy ability)
        {
            ability.ExecuteAbility(transform);
        }

        [Provide]
        public AbilityStrategy[] ProvideAbilities()
        {
            return m_abilities.ToArray();
        }
    }
}