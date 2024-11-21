using System.Collections.Generic;
using Code.Hero.Abilities;
using Code.UI.Factories;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Code.UI
{
    public class HeadsUpDisplay : MonoBehaviour
    {
        [SerializeField] private AbilityButtonFactory m_abilityButtonFactory;
        [SerializeField] private AbilityStrategy[] m_abilityStrategies;
        
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