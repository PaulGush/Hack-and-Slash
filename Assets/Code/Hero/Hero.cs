using System;
using Code.Hero.Abilities;
using Code.UI;
using DependencyInjection;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Hero
{
    public class Hero : MonoBehaviour, IDependencyProvider
    {
        public event Action<Ability> OnAbilityChanged;
        
        [SerializeField] private AttackAbility m_attackAbility;
        [SerializeField] private SpecialAbility m_specialAbility;
        [SerializeField] private BlockAbility m_blockAbility;
        [SerializeField] private DashAbility m_dashAbility;
        private void OnEnable()
        {
            AbilityButton.OnButtonPressed += ExecuteAbility;
        }

        private void OnDisable()
        {
            AbilityButton.OnButtonPressed -= ExecuteAbility;
        }

        private void ExecuteAbility(Ability ability)
        {
            ability.ExecuteAbility(transform);
        }

        [Button]
        public void AddAbility(Ability ability)
        {
            switch (ability)
            {
                case AttackAbility attackAbility:
                    m_attackAbility = attackAbility;
                    break;
                case SpecialAbility specialAbility:
                    m_specialAbility = specialAbility;
                    break;
                case BlockAbility blockAbility:
                    m_blockAbility = blockAbility;
                    break;
                case DashAbility dashAbility:
                    m_dashAbility = dashAbility;
                    break;
            }

            OnAbilityChanged?.Invoke(ability);
        }

        public Ability GetAttackAbility() => m_attackAbility;
        public Ability GetSpecialAbility() => m_specialAbility;
        public Ability GetBlockAbility() => m_blockAbility;
        public Ability GetDashAbility() => m_dashAbility;
        
        [Provide]
        public Hero ProvideHero()
        {
            return this;
        }
    }
}