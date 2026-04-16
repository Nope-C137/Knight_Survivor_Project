using UnityEngine;
using UnityEngine.InputSystem;

namespace TD
{
    [DefaultExecutionOrder(-2)]
    public class PlayerLocomotionInput : MonoBehaviour, PlayerInput.IPlayerMovementActions
    {
        [SerializeField] private bool holdToSprint = true;

        public bool SprintToggledOn { get; private set; }
        public bool WalkToggledOn { get; private set; }
        public bool JumpPressed { get; private set; }
        public PlayerInput playerInput { get; private set; }
        public Vector2 MovementInput { get; private set; }
        public Vector2 LookInput { get; private set; }

        private void OnEnable()
        {
            playerInput = new PlayerInput();
            playerInput.Enable();

            playerInput.PlayerMovement.Enable();
            playerInput.PlayerMovement.SetCallbacks(this);
        }

        private void OnDisable()
        {
            playerInput.PlayerMovement.Disable();
            playerInput.PlayerMovement.RemoveCallbacks(this);
        }

        private void LateUpdate()
        {
            JumpPressed = false;
        }

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
            {
                return;
            }

            JumpPressed = true;
        }

        public void OnWalking(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }

            WalkToggledOn = !WalkToggledOn;
        }
    }

}