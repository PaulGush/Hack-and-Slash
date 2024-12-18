using System;
using Code.Hero.Abilities;
using Code.Input;
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
            //AbilityButton.OnButtonPressed += ExecuteAbility;
            
            PlayerInputs.Instance.OnPrimaryPressed += PlayerInputs_OnPrimaryPressed;
            PlayerInputs.Instance.OnSecondaryPressed += PlayerInputs_OnSecondaryPressed;
            PlayerInputs.Instance.OnSpecialPressed += PlayerInputs_OnSpecialPressed;
            PlayerInputs.Instance.OnDashPressed += PlayerInputs_OnDashPressed;
            
            PlayerInputs.Instance.OnPrimaryReleased += PlayerInputs_OnPrimaryReleased;
            PlayerInputs.Instance.OnSecondaryReleased += PlayerInputs_OnSecondaryReleased;
            PlayerInputs.Instance.OnSpecialReleased += PlayerInputs_OnSpecialReleased;
            PlayerInputs.Instance.OnDashReleased += PlayerInputs_OnDashReleased;
        }

        private void OnDisable()
        {
            //AbilityButton.OnButtonPressed -= ExecuteAbility;
            
            PlayerInputs.Instance.OnPrimaryPressed -= PlayerInputs_OnPrimaryPressed;
            PlayerInputs.Instance.OnSecondaryPressed -= PlayerInputs_OnSecondaryPressed;
            PlayerInputs.Instance.OnSpecialPressed -= PlayerInputs_OnSpecialPressed;
            PlayerInputs.Instance.OnDashPressed -= PlayerInputs_OnDashPressed;
            
            PlayerInputs.Instance.OnPrimaryReleased -= PlayerInputs_OnPrimaryReleased;
            PlayerInputs.Instance.OnSecondaryReleased -= PlayerInputs_OnSecondaryReleased;
            PlayerInputs.Instance.OnSpecialReleased -= PlayerInputs_OnSpecialReleased;
            PlayerInputs.Instance.OnDashReleased -= PlayerInputs_OnDashReleased;
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

        private void PlayerInputs_OnPrimaryPressed()
        {
            ExecuteAbility(m_attackAbility);
        }

        private void PlayerInputs_OnPrimaryReleased()
        {
            
        }

        private void PlayerInputs_OnSecondaryPressed()
        {
            ExecuteAbility(m_specialAbility);
        }

        private void PlayerInputs_OnSecondaryReleased()
        {
            
        }

        private void PlayerInputs_OnSpecialPressed()
        {
            ExecuteAbility(m_blockAbility);
        }

        private void PlayerInputs_OnSpecialReleased()
        {
            
        }

        private void PlayerInputs_OnDashPressed()
        {
            ExecuteAbility(m_dashAbility);
        }

        private void PlayerInputs_OnDashReleased()
        {
            
        }
    }
}