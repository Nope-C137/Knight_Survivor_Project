using System.Globalization;
using UnityEngine;

namespace TD
{
    public class PlayerManager : CharacterManager
    {

        [HideInInspector] public PlayerAnimationManager playerAnimationManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;

        protected override void Awake()
        {
            base.Awake();

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimationManager = GetComponent<PlayerAnimationManager>();

        }

        protected override void Update()
        {
            base.Update();

            // Handle player movement
            playerLocomotionManager.HandleAllMovement();
        }

        protected override void LateUpdate()
        {
          
            PlayerCamera.instance.HandleAllCameraActions();
        }
    }

}