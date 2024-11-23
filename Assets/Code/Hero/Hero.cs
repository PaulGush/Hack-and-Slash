using System.Collections.Generic;
using Code.Hero.Abilities;
using Code.UI;
using UnityEngine;

namespace Code.Hero
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private AbilityStrategy m_primaryAbility;
        [SerializeField] private AbilityStrategy m_secondaryAbility;
        [SerializeField] private AbilityStrategy m_tertiaryAbility;
        [SerializeField] private AbilityStrategy m_dashAbility;
        
        [SerializeField] private List<AbilityStrategy> m_abilities = new List<AbilityStrategy>();
        private void OnEnable()
        {
            InitializeAbilities();
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

        public List<AbilityStrategy> GetAbilities() => m_abilities;

        private void InitializeAbilities()
        {
            m_abilities.Clear();
            
            m_abilities.Add(m_primaryAbility);
            m_abilities.Add(m_secondaryAbility);
            m_abilities.Add(m_tertiaryAbility);
            m_abilities.Add(m_dashAbility);
        }
    }
}