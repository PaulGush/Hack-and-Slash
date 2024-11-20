using System;
using Code.Hero.Abilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class AbilityButton : MonoBehaviour
    {
        [SerializeField] private AbilityStrategy m_ability;
        [SerializeField] private Image m_radial;
        [SerializeField] private TextMeshProUGUI m_text;
        [SerializeField] private Image m_icon;

        private Cooldown m_cooldown;
        
        private void Awake()
        {
            m_cooldown = m_ability.GetCooldown();
        }

        private void OnEnable()
        {
            UpdateIcon(m_ability.GetIcon());

            m_cooldown.OnCooldownBegin += Cooldown_OnCooldownBegin;
            m_cooldown.OnCooldownTimerTick += Cooldown_OnCooldownTimerTick;
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
            m_radial.fillAmount = amount / m_cooldownDuration;
        }

        private void UpdateText(float newValue)
        {
            var final = Mathf.RoundToInt(newValue);

            if (final == 0)
            {
                m_text.text = "";
                return;
            }

            m_text.text = final.ToString();
        }

        private void UpdateIcon(Sprite icon) => m_icon.sprite = icon;
    }
}
