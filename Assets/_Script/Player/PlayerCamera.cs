using TD;
using UnityEngine;

namespace TD
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera instance;
        public PlayerManager player;
        public Camera cameraObject;
        [SerializeField] Transform cameraPivotTransform; //THIS IS THE TRANSFORM THAT WILL ROTATE UP AND DOWN, WHILE THE CAMERA ROTATES LEFT AND RIGHT

        //CHANGE THIS TO TWEAK CAMERA PEFORMANCE
        [Header("Camera Settings")]
        [SerializeField] private float cameraSmoothSpeed = 1f;    //THE BIGGER THE NUMBER, THE LONGER FOR THE CAMERA TO REACH IT POSITION DURING MOVEMENT
        [SerializeField] float upAndDownRotationSpeed = 220; //SPEED OF UP AND DOWN CAMERA ROTATION
        [SerializeField] float leftAndRightRotationSpeed = 220; //SPEED OF LEFT AND RIGHT CAMERA ROTATION
        [SerializeField] float minimunPivot = -30; //THE LOWEST POIT YOU ARE ABLE TO LOOK DOWN AT
        [SerializeField] float maximumPivot = 60;//THE HIGHEST POIT YOU ARE ABLE TO LOOK DOWN AT
        [SerializeField] float cameraCollisionRadius = 0.2f;
        [SerializeField] LayerMask collideWithLayers; //LAYER MASK TO DETECT COLLISIONS WITH THE CAMERA

        //JUST DISPLAYS CAMERA VALUES
        [Header("Camera Values")]
        private Vector3 cameraVelocity;
        private Vector3 cameraObjectPosition; //VALUES USE FOR CAMERA COLLISION
        [SerializeField] float leftAndRightLookAngle;
        [SerializeField] float upAndDownLookAngle;
        private float cameraZPosition; //VALUES USE FOR CAMERA COLLISION 
        private float targetCameraZPosition;  //VALUES USE FOR CAMERA COLLISION 

        public GameObject focusCanvas;
        public void Awake()
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

            cameraZPosition = cameraObject.transform.localPosition.z; //GET THE DEFAULT CAMERA POSITION ON THE Z AXIS
                                                                      //Cursor.lockState = CursorLockMode.Locked;
                                                                      //Cursor.visible = false; //HIDE THE CURSOR

        }

        public void HandleAllCameraActions()
        {
            if (player != null)
            {
                HandleFollowTarget();
                HandleRotarions();
                HandleCollisions();
            }

        }

        private void HandleFollowTarget()
        {
            float smoothSpeed = player != null && player.characterLocomotionManager.isRolling ? 0.05f : cameraSmoothSpeed;

            Vector3 targetPosition = player.transform.position;

            Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position,
                targetPosition,
                ref cameraVelocity,
                smoothSpeed * Time.deltaTime);

            transform.position = targetCameraPosition;
        }

        private void HandleRotarions()
        {
            // IF WE LOCKED ON A TARGET, WE WANT TO FORCE ROTATE TOWARDS THAT TARGET
            //ELSE, WE WANT TO ROTATE NORMALLY

            //NORMAL ROTATION
            //ROTATE THE CAMERA BASED ON THE PLAYER INPUT (HORIZONTAL AND VERTICAL MOVEMENT INPUT)
            leftAndRightLookAngle += (PlayerInputManager.instance.camerahorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
            upAndDownLookAngle -= (PlayerInputManager.instance.cameraverticalInput * upAndDownRotationSpeed) * Time.deltaTime;
            //CLAMP THE UP AND DOWN LOOK ANGLE BETWEEN A MINIMUM AND MAXIMUM VALUE
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimunPivot, maximumPivot);

            Vector3 cameraRotation = Vector3.zero;
            Quaternion targetRotation;
            //ROTATE THIS GAMEOBJECT LEFT AND RIGHT
            cameraRotation.y = leftAndRightLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;

            //ROTATE THIS PIVOT GAMEOBJECT UP AND DOWN
            cameraRotation = Vector3.zero;
            cameraRotation.x = upAndDownLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            cameraPivotTransform.localRotation = targetRotation;
        }

        private void HandleCollisions()
        {
            targetCameraZPosition = cameraZPosition;
            RaycastHit hit;
            //DIRECTION FOR COLLISION CHECK
            Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
            direction.Normalize();

            //WE CHECK IF THERE IS AN OBJECT IN FRONT OF OUR DESIRE DIRECTION
            if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers))
            {
                //IF THERE IS, WE GET OUR DISTANCE FROM IT
                float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
            }


            //IF THE TARGET CAMERA POSITION IS TOO CLOSE TO THE CAMERA COLLISION RADIUS, WE SET IT TO THE NEGATIVE CAMERA COLLISION RADIUS
            if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
            {
                targetCameraZPosition = -cameraCollisionRadius;
            }

            cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
            cameraObject.transform.localPosition = cameraObjectPosition;
        }
    }

}