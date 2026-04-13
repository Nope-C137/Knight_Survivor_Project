using UnityEngine;

namespace TD
{
    public class characterLocomotionManager : MonoBehaviour
    {
        CharacterManager character;

        [Header("Ground Check & Jumping")]
        [SerializeField] protected float gravityForce = -9.81f; // THE FORCE OF GRAVITY APPLIED TO THE CHARACTER
        [SerializeField] float rollSpeed = 5f;
        [SerializeField] LayerMask groundLayer; // LAYER MASK FOR THE GROUND CHECK
        [SerializeField] float groundCheckSphereRadius = 1f; // RADIUS OF THE GROUND CHECK SPHERE
        [SerializeField] protected Vector3 yVelocity; // THE FORCE AT WHICH WE ARE MOVING UP OR DOWN(JUMPING OR FALLING GRAVITY BASICLLY)
        [SerializeField] protected float groundedYVelocity = -20f; // THE FORCE AT WHICH WE ARE GROUNDED 
        [SerializeField] protected float fallStartYVelocity = -5f; // THE FORCE AT WHICH WE START FALLING WHEN THEY BECOME UNGROUNDED (RISE AS THEY FALL LONGER)
        protected bool fallingVelocityHasBeenSet = false;
        [SerializeField] protected float inAirTimer = 0f; // THE TIME WE HAVE BEEN IN THE AIR

        [Header("Flags")]
        public bool isPerformingAction = false;
        public bool applyRootMotion = false;
        public bool isRolling = false;
        public bool canRotate = true;
        public bool canMove = true;
        public bool isGrounded = true;
        public bool isSprinting = false;
        public bool isJumping = false;

        [Header("Sliding")]
        public bool isSliding = false;
        public float slideSpeed = 6f;
        public float slideTimer = 0.4f;
        public float slideDuration = 0.1f;
        public Vector3 slideDirection;
        public Vector3 lastMovementDirection = Vector3.zero;

        protected virtual void Awake()
        {
            // Initialize any required components or variables here
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Update()
        {

            // Handle character movement and animations here
            // This method can be overridden in derived classes for specific behavior

            HandleGroundCheck();

            if (character.characterLocomotionManager.isGrounded)
            {
                //IF WE ARE NOT ATTEMPTING TO JUMP OR MOVE UPWARD
                if (yVelocity.y < 0f)
                {
                    inAirTimer = 0f; // RESET THE IN AIR TIMER
                    fallingVelocityHasBeenSet = false; // RESET THE FALLING VELOCITY FLAG
                    yVelocity.y = groundedYVelocity; // SET THE Y VELOCITY TO THE GROUNDED Y VELOCITY
                }
            }
            else
            {
                //IF WE ARE NOT JUMPING AND OUR FALLING VELOCITY HAS NOT BEEN SET
                if (!fallingVelocityHasBeenSet)
                {
                    fallingVelocityHasBeenSet = true; // SET THE FALLING VELOCITY FLAG TO TRUE
                    yVelocity.y = fallStartYVelocity; // SET THE Y VELOCITY TO THE FALL START Y VELOCITY

                }

                inAirTimer = inAirTimer + Time.deltaTime; // INCREMENT THE IN AIR TIMER
                character.animator.SetFloat("inAirTimer", inAirTimer); // SET THE IN AIR TIMER PARAMETER IN THE ANIMATOR
                yVelocity.y += gravityForce * Time.deltaTime; // APPLY GRAVITY TO THE Y VELOCITY

            }

            // ADD FORWARD MOVEMENT IF ROLLING
            Vector3 moveDirection = Vector3.zero;
            if (character.characterLocomotionManager.isRolling)
            {
                moveDirection = character.transform.forward * rollSpeed;
            }
            Vector3 finalMove = moveDirection + yVelocity;
            character.characterController.Move(finalMove * Time.deltaTime);

            //THERE SHOULD ALWAYS BE SOME FORCE APPILED TO THE Y VELOCITY, OTHERWISE THE CHARACTER WILL FLOATING 
            //character.characterController.Move(yVelocity * Time.deltaTime); // MOVE THE CHARACTER CONTROLLER BASED ON THE Y VELOCITY

        }

        protected void HandleGroundCheck()
        {
            character.characterLocomotionManager.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
        }

        protected void OnDrawGizmosSelected()
        {
            if (character == null)
                character = GetComponent<CharacterManager>();

            if (character != null)
                Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
        }

        public void EnableCanRotate()
        {
            canRotate = true;
        }

        public void DisableCanRotate()
        {
            canRotate = false;
        }

    }
}
