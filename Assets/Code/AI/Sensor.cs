using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityUtils;

namespace Code.AI
{
    [RequireComponent(typeof(Collider))]
    public class Sensor : MonoBehaviour
    {
        [SerializeField] private float m_detectionRadius = 5f;
        [SerializeField] private float m_timerInterval = 1f;
        
        [SerializeField] private Collider m_detectionRange;

        public event Action OnTargetChanged = delegate { };
        
        public Vector3 GetTargetPosition => m_target ? m_target.transform.position : Vector3.zero;
        public bool IsTargetInRange => GetTargetPosition != Vector3.zero;
        
        private GameObject m_target;
        private Vector3 m_lastKnownPosition;
        private CountdownTimer m_timer;

        private void Start()
        {
            m_timer = new CountdownTimer(m_timerInterval);
            m_timer.OnTimerStop += () =>
            {
                UpdateTargetPosition(m_target.OrNull());
                m_timer.Start();
            };
            m_timer.Start();
        }

        private void Update()
        {
            m_timer.Tick(Time.deltaTime);
        }

        void UpdateTargetPosition(GameObject target = null)
        {
            m_target = target;
            if (IsTargetInRange && (m_lastKnownPosition != GetTargetPosition || m_lastKnownPosition != Vector3.zero))
            {
                m_lastKnownPosition = GetTargetPosition;
                OnTargetChanged?.Invoke();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            UpdateTargetPosition(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            UpdateTargetPosition();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = IsTargetInRange ? Color.red : Color.green;
            Gizmos.DrawWireSphere(transform.position, m_detectionRadius);
        }
    }
}