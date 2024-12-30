using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Gameplay
{
    public class Chest : MonoBehaviour
    {
        [SerializeField] private Animator m_animator;

        public event Action OnOpened;

        [Button]
        public void Open()
        {
            m_animator.Play("Open");
            OnOpened?.Invoke();
        }

        [Button]
        public void Close()
        {
            m_animator.Play("Close");
        }
    }
}
