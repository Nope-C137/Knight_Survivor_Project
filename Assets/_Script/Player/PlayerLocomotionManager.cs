using UnityEngine;

namespace TD
{
    public class PlayerLocomotionManager : characterLocomotionManager
    {
        PlayerManager player;

        [HideInInspector] public float verticalMovement;
        [HideInInspector] public float horizontalMovement;
        [HideInInspector] public float moveAmount;

        [Header("Movement Settings")]
        private Vector3 movementDirection;
        private Vector3 targetRotationDirection;

        [SerializeField] float walkingSpeed = 2f;
        [SerializeField] float runningSpeed = 5f;
        [SerializeField] float sprintingSpeed = 6f;
        [SerializeField] float rotationSpeed = 15f;

        [Header("Jump Settings")]
        [SerializeField] float junpHeight = 4f; // THE HEIGHT OF THE JUMP
        [SerializeField] float jumpForwardSpeed = 5f; // THE FORWARD VELOCITY OF THE JUMP
        [SerializeField] float freeFallSpeed = 2f; // THE VELOCITY OF THE FREE FALL
        private Vector3 jumpDirection; // THE VELOCITY OF THE JUMP


        [Header("Dodge Settings")]
        private Vector3 rollDirection;
        public float backStepSpeed = 5f; // Speed of the backstep movement
        public float backStepDuration = 0.5f; // Duration of the backstep movement     

        [Header("FallToRoll Settings")]
        private float fallStartY = 0f;
        [SerializeField] float fallDistanceThreshold = 10f; // Minimum height to trigger roll
        private bool isFalling = false;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        protected override void Update()
        {
            base.Update();

            // Detect When Player Starts Falling
            if (!player.characterLocomotionManager.isGrounded && !isFalling)
            {
                if (inAirTimer > 0.1f) // A small delay to ensure we are actually falling
                {
                    isFalling = true;
                    fallStartY = transform.position.y; // Record the Y position when falling starts
                }
            }
            //Detect Landing and Measure Fall Distance
            if (player.characterLocomotionManager.isGrounded && isFalling)
            {
                float fallEndY = transform.position.y;
                float fallDistance = fallStartY - fallEndY;

                if (fallDistance >= fallDistanceThreshold)
                {
                    //PERFORM THE ROLL ANIMATION
                    rollDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
                    rollDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;

                    rollDirection.y = 0;
                    rollDirection.Normalize();

                    Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
                    player.transform.rotation = playerRotation;

                    player.playerAnimationManager.PlayerTargetActionAnimation("Rolling", true, true);
                    player.characterLocomotionManager.isRolling = true;
                }

                isFalling = false;
            }
        }

        public void HandleAllMovement()
        {
            GetMovementValues();
            HandleGroundedMovement();
            HandleJumpMovement();
            HandleFreeFallMovement();
            HandleRotation();
        }

        private void GetMovementValues()
        {
            verticalMovement = PlayerInputManager.instance.verticalInput;
            horizontalMovement = PlayerInputManager.instance.horizontalInput;
            moveAmount = PlayerInputManager.instance.moveAmount;

            //ClAMPS THE MOVEMENTS
        }

        private void HandleGroundedMovement()
        {
            if (player.characterLocomotionManager.canMove || player.characterLocomotionManager.canRotate)
            {
                GetMovementValues();

            }

            if (!player.characterLocomotionManager.canMove)
                return;

            //OUR MOVE DIRECTION IS BASED ON OUR CAMERA FACING DIRECTION & OUR MOVEMENT INPUT
            movementDirection = PlayerCamera.instance.transform.forward * verticalMovement;
            movementDirection = movementDirection + PlayerCamera.instance.transform.right * horizontalMovement;
            movementDirection.Normalize();
            movementDirection.y = 0;

            if (player.playerLocomotionManager.isSprinting)
            {
                player.characterController.Move(movementDirection * sprintingSpeed * Time.deltaTime);
            }
            else
            {
                if (PlayerInputManager.instance.moveAmount > 0.5f)
                {
                    //MOVE AT A RUNNING SPEED

                    player.characterController.Move(movementDirection * runningSpeed * Time.deltaTime);
                }
                else if (PlayerInputManager.instance.moveAmount <= 0.5f)
                {
                    //MOVE AT A WALKING SPEED

                    player.characterController.Move(movementDirection * walkingSpeed * Time.deltaTime);

                }
            }
        }
        private void HandleJumpMovement()
        {
            if (player.playerLocomotionManager.isJumping)
            {
                player.characterController.Move(jumpDirection * jumpForwardSpeed * Time.deltaTime);
            }
        }

        private void HandleFreeFallMovement()
        {
            if (!player.characterLocomotionManager.isGrounded)
            {

                Vector3 freeFallDirection;
                freeFallDirection = PlayerCamera.instance.transform.forward * PlayerInputManager.instance.verticalInput;
                freeFallDirection += PlayerCamera.instance.transform.right * PlayerInputManager.instance.horizontalInput;
                freeFallDirection.y = 0;

                player.characterController.Move(freeFallDirection * freeFallSpeed * Time.deltaTime);
            }
        }

        private void HandleRotation()
        {
            if (!player.characterLocomotionManager.canRotate)
                return;

            targetRotationDirection = Vector3.zero;
            targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
            targetRotationDirection = targetRotationDirection + PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
            targetRotationDirection.Normalize();
            targetRotationDirection.y = 0;

            if (targetRotationDirection == Vector3.zero)
            {
                targetRotationDirection = transform.forward;
            }

            Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }

        public void HandleSprinting()
        {
            if (player.characterLocomotionManager.isPerformingAction)
            {
                //SET SPRINTING TO FALSE
                player.playerLocomotionManager.isSprinting = false;
            }

            //IF WE ARE OUT OF STAMINA, THEN WE SET THE SPRINT TO FALSE


            //IF WE ARE MOVING, THEN WE SET THE SPRINT TO TRUE
            if (moveAmount >= 0.5)
            {
                player.playerLocomotionManager.isSprinting = true;

            }
            //IF WE NOT MOVING, THEN WE SET THE SPRINT TO FALSE
            else
            {
                player.playerLocomotionManager.isSprinting = false;

            }

        }

        public void AttemptToPerformDodge()
        {
            //PERFORM THE ROLL ANIMATION

            if (player.characterLocomotionManager.isPerformingAction)
                return;

            //IF WE ARE MOVING WHEN WE ATTEMPT TO DODGE, THEN WE PERFORM A ROLL, OTHERWISE WE PERFORM A BACK STEP
            if (PlayerInputManager.instance.moveAmount > 0)
            {
                //PERFORM THE ROLL ANIMATION
                rollDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
                rollDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;

                rollDirection.y = 0;
                rollDirection.Normalize();

                Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
                player.transform.rotation = playerRotation;

                player.playerAnimationManager.PlayerTargetActionAnimation("Rolling", true, true);
                player.characterLocomotionManager.isRolling = true;

            }
            else
            {
                //PERFORM THE BACK STEP ANIMATION
                player.playerAnimationManager.PlayerTargetActionAnimation("JumpBackward", true, true);
                //StartCoroutine(PerformBackstepMovement(backStepDuration, backStepSpeed)); // Adjust duration and speed as needed
            }

        }

        public void AttemptToPerformJump()
        {
            //IF WE ARE PERFORM THE JUMP ACTION, THEN WE DO NOT WANT TO ALLOW A JUMP

            if (player.characterLocomotionManager.isPerformingAction)
                return;

            //IF WE ARE JUMPING, THEN WE DO NOT WANT TO ALLOW A JUMP
            if (player.playerLocomotionManager.isJumping)
                return;

            //IF WE ARE NOT GROUNDED, THEN WE DO NOT WANT TO ALLOW A JUMP
            if (!player.characterLocomotionManager.isGrounded)
                return;

            player.playerAnimationManager.PlayerTargetActionAnimation("Main_JumpStart", false);

            player.playerLocomotionManager.isJumping = true; // Set the jumping flag to true

            jumpDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
            jumpDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;

            jumpDirection.y = 0;

            if (jumpDirection != Vector3.zero)
            {
                //IF WE ARE SPRINTING, JUMP DIRECTION IS AT FULL DISTANCE
                if (player.playerLocomotionManager.isSprinting)
                {
                    jumpDirection *= 1;
                }
                //IF WE ARE RUNING, JUMP DIRECTION IS AT HALF DISTANCE
                else if (PlayerInputManager.instance.moveAmount >= 0.5f)
                {
                    jumpDirection *= 0.5f;
                }
                //IF WE ARE WALKING, JUMP DIRECTION IS AT QUATER DISTANCE
                else if (PlayerInputManager.instance.moveAmount <= 0.5f)
                {
                    jumpDirection *= 0.25f;
                }
            }

        }
        public void ApplyJumpingVelocity()
        {
            yVelocity.y = Mathf.Sqrt(junpHeight * -2 * gravityForce);
        }
    }
}