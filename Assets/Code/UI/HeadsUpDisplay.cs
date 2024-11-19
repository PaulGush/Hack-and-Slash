using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class HeadsUpDisplay : MonoBehaviour
    {
        [SerializeField] private Button[] m_abilities;

        public delegate void ButtonPressedEvent(int index);
        
        public static event ButtonPressedEvent OnButtonPressed;

        private void Awake()
        {
            for (int i = 0; i < m_abilities.Length; i++)
            {
                int index = i;
                m_abilities[i].onClick.AddListener(() => OnButtonPressed?.Invoke(index));
            }
        }

        private void HandleButtonPress(int index) => OnButtonPressed?.Invoke(index);
    }
}