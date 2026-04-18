using UnityEngine;
using UnityEngine.InputSystem;

namespace TD
{
    [DefaultExecutionOrder(-2)]
    public class PlayerLocomotionInput : MonoBehaviour, PlayerInput.IPlayerMovementInputActions
    {
        #region Class Variables
        [SerializeField] private bool holdToSprint = true;
        public Vector2 MovementInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public bool JumpPressed { get; private set; }
        public bool SprintToggledOn { get; private set; }
        public bool WalkToggledOn { get; private set; }
        #endregion

        #region Startup
        private void OnEnable()
        {
            if(PlayerInputManager.Instance ?.playerInput == null)
            {
                Debug.LogError("PlayerInputManager instance or playerInput is null. cannot enable");
                return;
            }

            PlayerInputManager.Instance.playerInput.PlayerMovementInput.Enable();
            PlayerInputManager.Instance.playerInput.PlayerMovementInput.SetCallbacks(this);
        }

        private void OnDisable()
        {
            if (PlayerInputManager.Instance?.playerInput == null)
            {
                Debug.LogError("PlayerInputManager instance or playerInput is null. cannot disable");
                return;
            }

            PlayerInputManager.Instance.playerInput.PlayerMovementInput.Disable();
            PlayerInputManager.Instance.playerInput.PlayerMovementInput.RemoveCallbacks(this);
        }
        #endregion

        #region Late Update Logic
        private void LateUpdate()
        {
            JumpPressed = false;
        }
        #endregion

        #region Input Callbacks
        public void OnMovement(InputAction.CallbackContext context)
        {
            MovementInput = context.ReadValue<Vector2>();
            print(MovementInput);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookInput = context.ReadValue<Vector2>();
        }

        public void OnSprinting(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                SprintToggledOn = holdToSprint || !SprintToggledOn;
            }
            else if (context.canceled)
            {
                SprintToggledOn = !holdToSprint && SprintToggledOn;
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            JumpPressed = true;
        }

        public void OnWalk(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            WalkToggledOn = !WalkToggledOn;
        }
        #endregion
    }

}