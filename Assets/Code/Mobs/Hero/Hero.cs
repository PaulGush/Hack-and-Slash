using System;
using System.Collections.Generic;
using Code.Gameplay;
using Code.Input;
using Code.Mobs.Hero.Abilities;
using Code.Services;
using Code.UI;
using DependencyInjection;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

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

        [Title("Registered Services")]
        [SerializeField] private List<Object> m_services;
        
        private UIPromptService m_uiPromptService;
        
        [Provide]
        public Hero ProvideHero()
        {
            return this;
        }

        private void Awake()
        {
            ServiceLocator serviceLocator = ServiceLocator.For(this);
            foreach (Object service in m_services)
            {
                serviceLocator.Register<object>(service.GetType(), service);
            }
        }

        private void Start()
        {
            InitialiseServices();
        }

        private void InitialiseServices()
        {
            ServiceLocator serviceLocator = ServiceLocator.For(this);
            serviceLocator.Get(out UIPromptService uiService);
            m_uiPromptService = uiService;
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

            PlayerInputs.Instance.OnInteractPressed += PlayerInputs_OnInteractPressed;
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
            
            PlayerInputs.Instance.OnInteractPressed -= PlayerInputs_OnInteractPressed;

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

        private void PlayerInputs_OnInteractPressed()
        {
            TryInteract();
        }

        private void TryInteract()
        {
            m_currentInteractable?.Interact();
        }

        private IInteractable m_currentInteractable;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out IInteractable interactable))
            {
                m_currentInteractable = interactable;
                m_uiPromptService.ShowText(interactable.GetPromptText());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.TryGetComponent(out IInteractable interactable)) return;
            
            if (m_currentInteractable != null && interactable == m_currentInteractable)
            {
                m_currentInteractable = null;
                m_uiPromptService.ClearText();
            }
        }
    }
}