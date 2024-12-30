using System;
using Code.Mobs.Hero.Abilities;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Cooldown = Code.Utils.Cooldown;

namespace Code.UI
{
    public class AbilityButton : MonoBehaviour
    {
        public HeroAbility m_heroAbility;
        public Image Radial;
        public TextMeshProUGUI Text;
        public Image Icon;
        public Button ButtonComponent;

        private Cooldown m_cooldown;

        public static event Action<HeroAbility> OnButtonPressed; 
        
        public class Builder
        {
            private GameObject m_buttonPrefab;
            private HeroAbility m_strategy;
            private Cooldown m_cooldown;
            private Transform m_parent;

            public Builder WithPrefab(GameObject buttonPrefab)
            {
                m_buttonPrefab = buttonPrefab;
                return this;
            }

            public Builder WithAbility(HeroAbility heroAbility)
            {
                m_strategy = heroAbility;
                return this;
            }

            public Builder WithCooldown(Cooldown cooldown)
            {
                m_cooldown = cooldown;
                return this;
            }

            public Builder WithParent(Transform parent)
            {
                m_parent = parent;
                return this;
            }

            public AbilityButton Build()
            {
                var abilityButtonObject = Instantiate(m_buttonPrefab, m_parent);
                var abilityButton = abilityButtonObject.GetComponent<AbilityButton>();
                
                abilityButton.m_cooldown = m_cooldown;
                abilityButton.SetAbilityStrategy(m_strategy);

                abilityButton.Initialize();
                
                return abilityButton;
            }
        }

        private void Initialize()
        {
            m_cooldown = m_heroAbility.GetCooldown();
            UpdateIcon(m_heroAbility.GetIcon());

            m_cooldown.OnCooldownBegin += Cooldown_OnCooldownBegin;
            m_cooldown.OnCooldownTimerTick += Cooldown_OnCooldownTimerTick;
            
            ButtonComponent.onClick.AddListener(() => OnButtonPressed?.Invoke(m_heroAbility));
        }

        private float m_cooldownDuration;
        
        private void Cooldown_OnCooldownBegin(float newValue)
        {
            m_cooldownDuration = newValue;
        }

        private void Cooldown_OnCooldownTimerTick(float newValue)
        {
            UpdateRadial(newValue);
            UpdateText(newValue);
        }

        private void UpdateRadial(float amount)
        {
            Radial.fillAmount = amount / m_cooldownDuration;
        }

        private void UpdateText(float newValue)
        {
            var final = Mathf.RoundToInt(newValue);

            if (final == 0)
            {
                Text.text = "";
                return;
            }

            Text.text = final.ToString();
        }

        private void UpdateIcon(Sprite icon) => Icon.sprite = icon;

        private void SetAbilityStrategy(HeroAbility strategy) => m_heroAbility = strategy;
    }
}
