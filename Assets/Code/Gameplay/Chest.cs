using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Gameplay
{
    public class Chest : MonoBehaviour
    {
        [SerializeField] private Animator m_animator;

        [Button]
        public void Open()
        {
            m_animator.Play("Open");
        }

        [Button]
        public void Close()
        {
            m_animator.Play("Close");
        }
    }
}
