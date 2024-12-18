using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Code.Input
{
    public class PlayerInputs : MonoBehaviour
    {
        public static PlayerInputs Instance { get; private set; }

        public event Action OnPrimaryPressed;
        public event Action OnPrimaryHeld;
        public event Action OnPrimaryReleased;
        
        public event Action OnSecondaryPressed;
        public event Action OnSecondaryHeld;
        public event Action OnSecondaryReleased;
        
        public event Action OnInteractPressed;
        public event Action OnInteractHeld;
        public event Action OnInteractReleased;
        
        public event Action OnCursorMoved;
        public event Action OnRotate;
        public event Action OnRotateHeld;
        
        public event Action OnSprintPressed;
        public event Action OnSprintHeld;
        public event Action OnSprintReleased;
        
        public event Action OnDashPressed;
        public event Action OnDashHeld;
        public event Action OnDashReleased;

        public event Action OnSpecialPressed;
        public event Action OnSpecialReleased;

        public event Action<Vector2> OnMovePressed;
        public event Action<Vector2> OnMoveHeld;
        public event Action<Vector2> OnMoveReleased;

        public Vector2 MoveInput;
        public float ZoomInput;
        public float RotateInput;
        public Vector2 LookInput;

        public PlayerControls Controls;

        private Gamepad m_gamepad;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }

            if (Gamepad.current != null)
            {
                m_gamepad = Gamepad.current;
            }

            Controls = new PlayerControls();

            Controls.Actions.Primary.performed += PrimaryPressed;
            Controls.Actions.Primary.canceled += PrimaryReleased;
            Controls.Actions.Secondary.performed += SecondaryPressed;
            Controls.Actions.Secondary.canceled += SecondaryReleased;
            Controls.Actions.Interact.performed += InteractPressed;
            Controls.Actions.Interact.canceled += InteractReleased;
            Controls.Actions.Dash.performed += DashPressed;
            Controls.Actions.Dash.canceled += DashReleased;
            Controls.Actions.Special.performed += SpecialPressed;
            Controls.Actions.Special.canceled += SpecialReleased;
            
            Controls.Movement.CursorMove.performed += CursorMoved;
            Controls.Movement.Rotate.performed += RotatePressed;
            Controls.Movement.Sprint.performed += SprintPressed;
            Controls.Movement.Sprint.canceled += SprintReleased;
            
            Controls.Movement.Move.performed += MovePressed;
            Controls.Movement.Move.canceled += MoveReleased;
        }

        private void CursorMoved(InputAction.CallbackContext context)
        {
            OnCursorMoved?.Invoke();
        }

        private void PrimaryPressed(InputAction.CallbackContext context)
        {
            OnPrimaryPressed?.Invoke();
        }

        private void PrimaryReleased(InputAction.CallbackContext context)
        {
            OnPrimaryReleased?.Invoke();
        }

        private void SecondaryPressed(InputAction.CallbackContext context)
        {
            OnSecondaryPressed?.Invoke();
        }

        private void SecondaryReleased(InputAction.CallbackContext context)
        {
            OnSecondaryReleased?.Invoke();
        }

        private void InteractPressed(InputAction.CallbackContext context)
        {
            OnInteractPressed?.Invoke();
        }

        private void InteractReleased(InputAction.CallbackContext context)
        {
            OnInteractReleased?.Invoke();
        }

        private void DashPressed(InputAction.CallbackContext context)
        {
            OnDashPressed?.Invoke();
        }

        private void DashReleased(InputAction.CallbackContext context)
        {
            OnDashReleased?.Invoke();
        }

        private void SprintPressed(InputAction.CallbackContext context)
        {
            OnSprintPressed?.Invoke();
        }

        private void SprintReleased(InputAction.CallbackContext context)
        {
            OnSprintReleased?.Invoke();
        }
        
        private void SpecialPressed(InputAction.CallbackContext context)
        {
            OnSpecialPressed?.Invoke();
        }
        
        private void SpecialReleased(InputAction.CallbackContext context)
        {
            OnSpecialReleased?.Invoke();
        }
        
        private void MovePressed(InputAction.CallbackContext context)
        {
            OnMovePressed?.Invoke(context.ReadValue<Vector2>());
        }
        
        private void MoveReleased(InputAction.CallbackContext context)
        {
            OnMoveReleased?.Invoke(context.ReadValue<Vector2>());
        }

        private void RotatePressed(InputAction.CallbackContext context)
        {
            if (context.ReadValue<float>() > 0)
            {
                RotateInput = 1;
            }
            else if (context.ReadValue<float>() < 0)
            {
                RotateInput = -1;
            }
            OnRotate?.Invoke();
        }

        #region Enabling/Disabling Action Maps
        private void Start()
        {
            Controls.Enable();
        }

        private void OnEnable()
        {
            Controls.Enable();
        }

        private void OnDisable()
        {
            Controls.Disable();
        }

        private void OnDestroy()
        {
            Controls.Disable();
        }
        #endregion

        private void Update()
        {
            HandleInputs();
        }

        private void HandleInputs()
        {
            MoveInput = Controls.Movement.Move.ReadValue<Vector2>();
            LookInput = Controls.Movement.Look.ReadValue<Vector2>();
            ZoomInput = Controls.Movement.Zoom.ReadValue<float>();

            // Reset RotateInput to prevent continuous rotation
            RotateInput = 0;

            if (Controls.Actions.Primary.phase == InputActionPhase.Performed)
            {
                OnPrimaryHeld?.Invoke();
            }

            if (Controls.Actions.Secondary.phase == InputActionPhase.Performed)
            {
                OnSecondaryHeld?.Invoke();
            }

            if (Controls.Actions.Interact.phase == InputActionPhase.Performed)
            {
                OnInteractHeld?.Invoke();
            }
            
            if (Controls.Actions.Dash.phase == InputActionPhase.Performed)
            {
                OnDashHeld?.Invoke();
            }

            if (Controls.Movement.Rotate.phase == InputActionPhase.Performed)
            {
                OnRotateHeld?.Invoke();
            }
            
            if (Controls.Movement.Sprint.phase == InputActionPhase.Performed)
            {
                OnSprintHeld?.Invoke();
            }

            if (Controls.Movement.Move.phase == InputActionPhase.Performed)
            {
                OnMoveHeld?.Invoke(Controls.Movement.Move.ReadValue<Vector2>());
            }
        }

        #region Haptics

        public void StartHapticFeedback(float intensity, float frequency)
        {
            m_gamepad?.SetMotorSpeeds(intensity, frequency);
        }

        public void StopHapticFeedback()
        {
            m_gamepad?.SetMotorSpeeds(0, 0);
        }

        #endregion
    }
}