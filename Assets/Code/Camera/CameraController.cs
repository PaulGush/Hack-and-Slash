using Code.Input;
using System.Collections;
using UnityEngine;

namespace Code.Camera
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance;

        [Header("References"), SerializeField] private UnityEngine.Camera m_camera;
        [Header("Values"), SerializeField] private float m_translateSpeed = 1f;
        [SerializeField] private float m_rotateSpeed = 5f;
        [SerializeField] private float m_rotateAngleIncrement = 90f;
        [Header("Zoom"), SerializeField] private float m_zoomSpeed = 1f;
        [SerializeField] private float m_maxZoomIn;
        [SerializeField] private float m_maxZoomOut;
        [SerializeField] private float m_defaultZoom;

        private float m_targetRotationY;
        private bool m_isRotating = false;

        private void Awake()
        {
            Instance = this;
        }

        private void LateUpdate()
        {
            Translate(PlayerInputs.Instance.MoveInput);
            Zoom(PlayerInputs.Instance.ZoomInput);
            PlayerInputs.Instance.OnRotate += IncrementalRotate;
        }

        private void Translate(Vector2 newVal)
        {
            var translation = new Vector3(newVal.x, 0f, newVal.y) * (m_translateSpeed * Time.deltaTime);

            transform.Translate(translation, Space.Self);
        }

        private void Zoom(float newVal)
        {
            var zoomDelta = m_zoomSpeed * newVal;
            var newOrthographicSize = m_camera.orthographicSize + zoomDelta;
            newOrthographicSize = Mathf.Clamp(newOrthographicSize, m_maxZoomIn, m_maxZoomOut);
            m_camera.orthographicSize = newOrthographicSize;
        }

        private void IncrementalRotate()
        {
            if (!m_isRotating)
            {
                m_targetRotationY += m_rotateAngleIncrement * Mathf.Sign(PlayerInputs.Instance.RotateInput);
                m_targetRotationY = Mathf.Round(m_targetRotationY / m_rotateAngleIncrement) * m_rotateAngleIncrement; // Ensure it snaps to degree increments
                StartCoroutine(SmoothRotate());
            }
        }

        private IEnumerator SmoothRotate()
        {
            m_isRotating = true;
            var initialRotation = transform.rotation;
            var targetRotation = Quaternion.Euler(transform.rotation.x, m_targetRotationY, transform.rotation.z);
            var elapsedTime = 0f;

            while (elapsedTime < 1f)
            {
                transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime);
                elapsedTime += Time.deltaTime * m_rotateSpeed;
                yield return null;
            }

            transform.rotation = targetRotation;
            m_isRotating = false;
        }

        public void MoveToPoint(Vector3 point)
        {
            StopAllCoroutines();
            StartCoroutine(Move(point));
        }

        private IEnumerator Move(Vector3 point)
        {
            while (Vector3.Distance(transform.position, point) > 0.001f)
            {
                transform.position = Vector3.MoveTowards(transform.position, point, m_translateSpeed * Time.deltaTime);
                yield return null;
            }

            transform.position = point;
        }
    }
}
