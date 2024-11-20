using System.Collections;
using Code.Hero.Abilities;
using Code.Input;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Hero
{
    public class HeroLocomotion : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody m_rigidbody;
        
        [Header("Values")]
        [SerializeField] private float m_moveSpeed = 5f;

        [SerializeField] private float m_rotationSpeed = 10f;

        private void Awake()
        {
            DashAbilityStrategy.OnDash += DashAbilityStrategy_OnDash;
        }

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
            
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * m_rotationSpeed);

            #endregion
            
        }

        private void DashAbilityStrategy_OnDash(float force, float duration)
        {
            StartCoroutine(SmoothDash(force, duration));
        }
        
        IEnumerator SmoothDash(float dashForce, float duration)
        {
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                
                m_rigidbody.AddForce(transform.forward * dashForce, ForceMode.Impulse);

                yield return null;
            }
        }
    }
}