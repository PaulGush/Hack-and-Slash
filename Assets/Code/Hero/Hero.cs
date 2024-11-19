using Code.Hero.Abilities;
using Code.UI;
using UnityEngine;

namespace Code.Hero
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] AbilityStrategy[] m_abilities;
        
        private void OnEnable()
        {
            HeadsUpDisplay.OnButtonPressed += ExecuteAbility;
        }

        private void OnDisable()
        {
            HeadsUpDisplay.OnButtonPressed -= ExecuteAbility;
        }

        void ExecuteAbility(int index)
        {
            m_abilities[index].ExecuteAbility(transform);
        }
    }
}