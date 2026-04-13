using TD;
using UnityEngine;

public class PlayerAnimationManager : CharacterAnimatorManager
{
    PlayerManager player;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    private void OnAnimatorMove()
    {
        if (player.characterLocomotionManager.applyRootMotion)
        {
            Vector3 velocity = player.animator.deltaPosition;
            player.characterController.Move(velocity);

            //player.transform.rotation *= player.animator.deltaRotation;

        }
    }
}
