using System;
using Code.Input;
using UnityEngine;

namespace Code.Hero
{
    public class HeroLocomotion : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody m_rigidbody;
        
        [Header("Values")]
        [SerializeField] private float m_moveSpeed = 5f;

        [SerializeField] private float m_rotationSoeed = 10f;
        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            var moveInput = PlayerInputs.Instance.MoveInput;

            if (moveInput == Vector2.zero)
            {
                m_rigidbody.linearVelocity = Vector3.zero;
                return;
            }

            m_rigidbody.linearVelocity = new Vector3(moveInput.x, 0f, moveInput.y) * m_moveSpeed;

            #region Rotation

            Vector3 newRotationVector = new Vector3(moveInput.x, 0f, moveInput.y);
            
            Quaternion targetRotation = Quaternion.LookRotation(newRotationVector);
            
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * m_rotationSoeed);

            #endregion
            
        }
    }
}