using System.Collections.Generic;
using Code.Hero.Abilities;
using Code.UI.Factories;
using DependencyInjection;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class HeadsUpDisplay : MonoBehaviour
    {
        [SerializeField] AbilityStrategy m_primaryAbility;
        [SerializeField] AbilityStrategy m_secondaryAbility;
        [SerializeField] AbilityStrategy m_tertiaryAbility;
        [SerializeField] AbilityStrategy m_dashAbility;
        
        [SerializeField] private AbilityButtonFactory m_abilityButtonFactory;
        [Inject, SerializeField] private AbilityStrategy[] m_abilityStrategies;
        
        [SerializeField] private List<Button> m_buttons;

        private void Awake()
        {
            foreach (var ability in m_abilityStrategies)
            {
                m_abilityButtonFactory.CreateButton(ability, transform);
            }
        }
    }
}