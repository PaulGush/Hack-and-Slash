using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Gameplay
{
    public class Chest : MonoBehaviour, IInteractable
    {
        [SerializeField] private Animator m_animator;
        private bool m_isOpen;
        
        [Button]
        public void Open()
        {
            m_animator.Play("Open");
            m_isOpen = true;
        }

        [Button]
        public void Close()
        {
            m_animator.Play("Close");
            m_isOpen = false;
        }

        public void Interact()
        {
            if (!m_isOpen)
            {
                Open();
            }
            else
            {
                Close();
            }
        }
        public string GetPromptText()
        {
            if (m_isOpen)
            {
                return "Press F to close";
            }
            else
            {
                return "Press F to open";
            }
        }
    }
}
