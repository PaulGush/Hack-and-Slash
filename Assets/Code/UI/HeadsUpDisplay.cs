using Code.Mobs.Hero.Abilities;
using Code.UI.Factories;
using DependencyInjection;
using UnityEngine;

namespace Code.UI
{
    public class HeadsUpDisplay : MonoBehaviour
    {
        [Inject, SerializeField] private Mobs.Hero.Hero m_hero;
        [SerializeField] private AbilityButtonFactory m_abilityButtonFactory;
        [SerializeField] private Transform[] m_abilityButtonsParent;

        private void Awake()
        {
            UpdateAllAbilities();
            m_hero.OnAbilityChanged += Hero_OnAbilityChanged;
        }

        private void Hero_OnAbilityChanged(HeroAbility heroAbility)
        {
            UpdateAbility(heroAbility);
        }

        private void UpdateAllAbilities()
        {
            UpdateAbility(m_hero.GetAttackAbility());
            UpdateAbility(m_hero.GetSpecialAbility());
            UpdateAbility(m_hero.GetBlockAbility());
            UpdateAbility(m_hero.GetDashAbility());
        }

        private void UpdateAbility(HeroAbility heroAbility)
        {
            switch (heroAbility)
            {
                case AttackHeroAbility:
                    DestroyChild(m_abilityButtonsParent[0]);
                    m_abilityButtonFactory.CreateButton(heroAbility, m_abilityButtonsParent[0]);
                    break;
                
                case SpecialHeroAbility:
                    DestroyChild(m_abilityButtonsParent[1]);
                    m_abilityButtonFactory.CreateButton(heroAbility, m_abilityButtonsParent[1]);
                    break;
                
                case BlockHeroAbility:
                    DestroyChild(m_abilityButtonsParent[2]);
                    m_abilityButtonFactory.CreateButton(heroAbility, m_abilityButtonsParent[2]);
                    break;
                
                case DashHeroAbility:
                    DestroyChild(m_abilityButtonsParent[3]);
                    m_abilityButtonFactory.CreateButton(heroAbility, m_abilityButtonsParent[3]);
                    break;
            }
        }

        private void DestroyChild(Transform parent)
        {
            if (parent.childCount != 0)
            {
                Destroy(parent.GetChild(0).gameObject);
            }
        }
    }
}