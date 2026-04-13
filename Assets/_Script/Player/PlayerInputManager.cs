using UnityEngine;
using UnityEngine.InputSystem;

namespace TD
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;
        public PlayerManager player;

        PlayerInput playerInputs;           //THIS IS PLAYERCONTROLS 
        [Header("Movement Input")]
        [SerializeField] Vector2 movementInput;
        public float horizontalInput;
        public float verticalInput;
        public float moveAmount;

        [Header("Camera Movement Input")]
        [SerializeField] Vector2 cameraInput;
        public float camerahorizontalInput;
        public float cameraverticalInput;

        [Header("Player Action Input")]
        [SerializeField] bool dodgeInput = false;
        [SerializeField] bool sprintInput = false;
        [SerializeField] bool jumpInput = false;
        //[SerializeField] bool interactInput = false; //USED FOR INTERACTING WITH OBJECTS, NPCS, ITEMS, FOGS WALLS, ELEVATORS ETC.

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }


        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            if (playerInputs == null)
            {
                playerInputs = new PlayerInput();

                playerInputs.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                playerInputs.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
                playerInputs.PlayerActions.Dodge.performed += i => dodgeInput = true;
                playerInputs.PlayerActions.Jump.performed += i => jumpInput = true;

            }

            playerInputs.Enable();
        }

        //IF WE MINIMIZE THE GAME OR LOWER THE WINDOW, WE WANT TO STOP AJUSTING THE MOVEMENT INPUT
        private void OnApplicationFocus(bool focus)
        {
            if (enabled)
            {
                if (focus)
                {
                    playerInputs.Enable();
                }
                else
                {
                    playerInputs.Disable();
                }
            }
        }

        private void Update()
        {
            HandleAllTickInputs();
        }


        private void HandleAllTickInputs()
        {
            HandlePlayerMovementInput();
            HandleCameraMovementInput();
            HandleDodgeInput();
            HandleSprintInput();
            HandleJumpInput();
        }

        private void HandlePlayerMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;

            //RETRUN THE AMOUNT OF MOVEMENT INPUT, (MEANING THE NUMBER WITHOUT THE NEGATIVE SIGN, SO IT'S ALWAYS POSITIVE)
            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

            if (moveAmount > 0)
            {
                Vector3 inputDirection = new Vector3(horizontalInput, 0, verticalInput);

                // Get camera-relative directions
                Vector3 cameraForward = Camera.main.transform.forward;
                Vector3 cameraRight = Camera.main.transform.right;

                // Flatten to horizontal plane
                cameraForward.y = 0;
                cameraRight.y = 0;
                cameraForward.Normalize();
                cameraRight.Normalize();

                // Calculate movement direction relative to camera
                Vector3 moveDirection = (cameraForward * inputDirection.z + cameraRight * inputDirection.x).normalized;

                // Only update if there's meaningful input
                if (moveDirection.magnitude > 0.1f)
                {
                    player.characterLocomotionManager.lastMovementDirection = moveDirection;
                }
            }

            //WE CLAMB THE VALUES SO THEY ART 0, 0.5, OR 1
            if (moveAmount <= 0.5f && moveAmount > 0)
            {
                moveAmount = 0.5f;
            }

            else if (moveAmount > 0.5f && moveAmount <= 1)
            {
                moveAmount = 1f;
            }

            //WHY DO WE PASS IN 0 FOR THE HORIZONTAL VALUE? BECAUSE WE ONLY WANT TO NON-STAFING MOVEMENT
            //WE USE THE HORIZONTAL WHEN WE ARE STAFING OR LOCKED ON

            if (player == null)
            {
                return;
            }

            //IF WEA ARE NOT LOCKED ON, ONLY USE THE MOVEMENT AMOUNT
            player.playerAnimationManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerLocomotionManager.isSprinting);

            //IF WE ARE LOCKED ON, PASS THE HORIZONTAL MOVEMENT AS WELL

            // Detect stop
            bool isCurrentlyMoving = movementInput.magnitude > 0.1f;

            if (player.characterLocomotionManager.isSliding)
            {
                if (player.characterLocomotionManager.slideTimer > 0)
                {
                    player.characterController.Move(player.characterLocomotionManager.slideDirection * player.characterLocomotionManager.slideSpeed * Time.deltaTime);

                    // Rotate player to face slide direction
                    Quaternion targetRotation = Quaternion.LookRotation(player.characterLocomotionManager.slideDirection);
                    player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, Time.deltaTime * 10f);

                    player.characterLocomotionManager.slideTimer -= Time.deltaTime;
                }
                else
                {
                    player.characterLocomotionManager.slideTimer = 0.5f; // CLAMP THE SLIDETIMER
                    player.characterLocomotionManager.isSliding = false;
                }
            }

            if (!isCurrentlyMoving)
            {
                if (player.playerLocomotionManager.isSprinting)
                {
                    player.animator.ResetTrigger("HardStopTrigger");
                    player.animator.SetTrigger("HardStopTrigger");

                    // Use last movement direction for slide
                    Vector3 slideDir = player.characterLocomotionManager.lastMovementDirection;
                    if (slideDir.magnitude < 0.1f)
                    {
                        // Fallback to forward if no movement was detected
                        slideDir = player.transform.forward;
                    }

                    player.characterLocomotionManager.slideDirection = slideDir.normalized;
                    player.characterLocomotionManager.slideTimer = player.playerLocomotionManager.slideDuration;
                    player.characterLocomotionManager.isSliding = true;
                }
                else
                {
                    player.animator.ResetTrigger("MediumStopTrigger");
                    player.animator.SetTrigger("MediumStopTrigger");
                }
            }
        }

        //CAMERA
        private void HandleCameraMovementInput()
        {
            cameraverticalInput = cameraInput.y;
            camerahorizontalInput = cameraInput.x;


        }

        //ACTION
        private void HandleDodgeInput()
        {
            if (dodgeInput)
            {
                dodgeInput = false; //RESET THE DODGE INPUT

                //PERFORM THE DODGE ACTION
                player.playerLocomotionManager.AttemptToPerformDodge();
            }
        }

        private void HandleSprintInput()
        {
            if (sprintInput)
            {
                player.playerLocomotionManager.HandleSprinting();
            }
            else
            {
                player.playerLocomotionManager.isSprinting = false; //STOP SPRINTING

            }
        }

        private void HandleJumpInput()
        {
            if (jumpInput)
            {
                jumpInput = false; //RESET THE JUMP INPUT

                //IF WE HAVE A UI WINDOW OPEN, RETURN AND DO NOTHING
                //if (PlayerUiManager.instance.menuWindowIsOpen)
                    //return;

                //ATTEMPT TO PERFORM JUMP
                player.playerLocomotionManager.AttemptToPerformJump();

            }
        }
    }

}