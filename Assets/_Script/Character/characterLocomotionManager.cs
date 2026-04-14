using UnityEngine;

namespace TD
{
    [RequireComponent(typeof(CharacterManager))]
    public class characterLocomotionManager : MonoBehaviour
    {
        CharacterManager character;

        [Header("Ground Check & Jumping")]
        [SerializeField] protected float gravityForce = -9.81f; // THE FORCE OF GRAVITY APPLIED TO THE CHARACTER
        [SerializeField] float rollSpeed = 5f;
        [SerializeField] LayerMask groundLayer; // LAYER MASK FOR THE GROUND CHECK
        [SerializeField] float groundCheckSphereRadius = 0.2f; // reduced radius for feet check
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
        public bool isGrounded = false; // start false to avoid initial glitch
        public bool isSprinting = false;
        public bool isJumping = false;

        [Header("Sliding")]
        public bool isSliding = false;
        public float slideSpeed = 6f;
        public float slideTimer = 0.4f;
        public float slideDuration = 0.1f;
        public Vector3 slideDirection;
        public Vector3 lastMovementDirection = Vector3.zero;

        [Header("Grounding Stability")]
        [SerializeField] int groundedStabilityFrames = 3; // how many frames required to consider grounded/un-grounded stable
        int groundedFrameCounter;

        protected virtual void Awake()
        {
            // Try to find the CharacterManager on this GameObject or a parent
            character = GetComponent<CharacterManager>();
            if (character == null)
            {
                character = GetComponentInParent<CharacterManager>();
            }

            if (character == null)
            {
                Debug.LogError("CharacterManager not found for characterLocomotionManager on " + gameObject.name);
                return;
            }

            // Register this locomotion manager on the CharacterManager so other code can access it
            character.characterLocomotionManager = this;
        }

        protected virtual void Update()
        {

            // Handle character movement and animations here
            // This method can be overridden in derived classes for specific behavior

            HandleGroundCheck();

            if (character != null && character.characterLocomotionManager.isGrounded)
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

                if (character != null && character.animator != null)
                    character.animator.SetFloat("inAirTimer", inAirTimer); // SET THE IN AIR TIMER PARAMETER IN THE ANIMATOR

                yVelocity.y += gravityForce * Time.deltaTime; // APPLY GRAVITY TO THE Y VELOCITY

            }

            // ADD FORWARD MOVEMENT IF ROLLING
            Vector3 moveDirection = Vector3.zero;
            if (character != null && character.characterLocomotionManager.isRolling)
            {
                moveDirection = character.transform.forward * rollSpeed;
            }
            Vector3 finalMove = moveDirection + yVelocity;

            if (character != null && character.characterController != null)
                character.characterController.Move(finalMove * Time.deltaTime);

            //THERE SHOULD ALWAYS BE SOME FORCE APPILED TO THE Y VELOCITY, OTHERWISE THE CHARACTER WILL FLOATING 
            //character.characterController.Move(yVelocity * Time.deltaTime); // MOVE THE CHARACTER CONTROLLER BASED ON THE Y VELOCITY

        }

        protected void HandleGroundCheck()
        {
            if (character == null)
            {
                character = GetComponent<CharacterManager>();
                if (character == null)
                    return;
                character.characterLocomotionManager = this;
            }

            // compute a check position near the feet, not at the object's pivot
            Vector3 checkPos;
            if (character.characterController != null)
            {
                // characterController.center is local; convert to world
                checkPos = character.transform.position + character.characterController.center + Vector3.down * (character.characterController.height * 0.5f - 0.05f);
            }
            else
            {
                checkPos = character.transform.position + Vector3.down * 0.1f;
            }

            // Use OverlapSphere and ignore colliders that belong to the character to avoid self-detection
            Collider[] hits = Physics.OverlapSphere(checkPos, groundCheckSphereRadius, groundLayer, QueryTriggerInteraction.Ignore);
            bool foundGround = false;

            for (int i = 0; i < hits.Length; i++)
            {
                var hit = hits[i];
                if (hit == null) continue;

                // ignore any collider that is the character or its children
                if (hit.transform == character.transform || hit.transform.IsChildOf(character.transform))
                    continue;

                foundGround = true;
                break;
            }

            // simple stability buffer to prevent single-frame flicker
            if (foundGround)
            {
                groundedFrameCounter = Mathf.Min(groundedFrameCounter + 1, groundedStabilityFrames);
            }
            else
            {
                groundedFrameCounter = Mathf.Max(groundedFrameCounter - 1, 0);
            }

            bool stableGrounded = groundedFrameCounter > 0;
            isGrounded = stableGrounded;

            // Keep CharacterManager's reference in sync (safe-guard)
            character.characterLocomotionManager = this;
            character.characterLocomotionManager.isGrounded = isGrounded;
        }

        protected void OnDrawGizmosSelected()
        {
            if (character == null)
                character = GetComponent<CharacterManager>();

            if (character != null)
            {
                Vector3 checkPos;
                if (character.characterController != null)
                    checkPos = character.transform.position + character.characterController.center + Vector3.down * (character.characterController.height * 0.5f - 0.05f);
                else
                    checkPos = character.transform.position + Vector3.down * 0.1f;

                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(checkPos, groundCheckSphereRadius);
            }
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
