using System;
using Code.Gameplay;
using Code.Input;
using Code.Mobs.Hero.Abilities;
using DependencyInjection;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Mobs.Hero
{
    public class Hero : MonoBehaviour, IDependencyProvider, IEntity
    {
        public event Action<HeroAbility> OnAbilityChanged;
        public event Action<HeroAbility> OnAbilityExecuted;
        
        [SerializeField] private HealthSystem m_healthSystem;
        
        [SerializeField] private AttackHeroAbility m_attackHeroAbility;
        [SerializeField] private SpecialHeroAbility m_specialHeroAbility;
        [SerializeField] private BlockHeroAbility m_blockHeroAbility;
        [SerializeField] private DashHeroAbility m_dashHeroAbility;
        
        [Provide]
        public Hero ProvideHero()
        {
            return this;
        }
        
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

        private void ExecuteAbility(HeroAbility heroAbility)
        {
            if (heroAbility.Type == HeroAbility.ExecuteType.single)
            {
                if (heroAbility.ExecuteAbility(transform)) OnAbilityExecuted?.Invoke(heroAbility);
            }
            else
            {
                heroAbility.ExecuteAbility(transform); 
                OnAbilityExecuted?.Invoke(heroAbility);
            }
        }

        /// <summary>
        /// Adds or replaces ability if an ability is not in that slot already.
        /// </summary>
        /// <param name="heroAbility"></param>
        [Button]
        public void AddAbility(HeroAbility heroAbility)
        {
            switch (heroAbility)
            {
                case AttackHeroAbility attackAbility:
                    m_attackHeroAbility = attackAbility;
                    break;
                case SpecialHeroAbility specialAbility:
                    m_specialHeroAbility = specialAbility;
                    break;
                case BlockHeroAbility blockAbility:
                    m_blockHeroAbility = blockAbility;
                    break;
                case DashHeroAbility dashAbility:
                    m_dashHeroAbility = dashAbility;
                    break;
            }

            OnAbilityChanged?.Invoke(heroAbility);
        }

        public HeroAbility GetAttackAbility() => m_attackHeroAbility;
        public HeroAbility GetSpecialAbility() => m_specialHeroAbility;
        public HeroAbility GetBlockAbility() => m_blockHeroAbility;
        public HeroAbility GetDashAbility() => m_dashHeroAbility;

        private void PlayerInputs_OnPrimaryPressed()
        {
            ExecuteAbility(m_attackHeroAbility);
        }

        private void PlayerInputs_OnPrimaryReleased()
        {
            
        }

        private void PlayerInputs_OnSecondaryPressed()
        {
            ExecuteAbility(m_specialHeroAbility);
        }

        private void PlayerInputs_OnSecondaryReleased()
        {
            
        }

        private void PlayerInputs_OnSpecialPressed()
        {
            ExecuteAbility(m_blockHeroAbility);
        }

        private void PlayerInputs_OnSpecialReleased()
        {
            
        }

        private void PlayerInputs_OnDashPressed()
        {
            ExecuteAbility(m_dashHeroAbility);
        }

        private void PlayerInputs_OnDashReleased()
        {
            
        }
    }
}