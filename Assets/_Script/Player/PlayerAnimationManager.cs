using TD;
using UnityEngine;

public class PlayerAnimationManager : CharacterAnimatorManager
{
    PlayerManager player;

    protected override void Awake()
    {
        base.Awake();

        // Try same object first, then parent, then children to avoid null refs
        player = GetComponent<PlayerManager>();
        if (player == null)
            player = GetComponentInParent<PlayerManager>();
        if (player == null)
            player = GetComponentInChildren<PlayerManager>();

        if (player == null)
            Debug.LogWarning("PlayerManager component not found for PlayerAnimationManager on " + gameObject.name);
    }

    private void OnAnimatorMove()
    {
        // Guard against missing references to avoid NullReferenceException
        if (player?.characterLocomotionManager?.applyRootMotion == true)
        {
            Vector3 velocity = player?.animator?.deltaPosition ?? Vector3.zero;

            if (player.characterController != null)
            {
                player.characterController.Move(velocity);
            }
            else
            {
                Debug.LogWarning("CharacterController is null on PlayerManager attached to " + (player != null ? player.name : "null"));
            }

            //player.transform.rotation *= player.animator.deltaRotation;
        }
    }
}
