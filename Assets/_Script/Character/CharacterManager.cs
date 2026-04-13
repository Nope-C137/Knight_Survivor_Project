using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace TD
{
    public class CharacterManager : MonoBehaviour
    {
        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;
        [HideInInspector] public characterLocomotionManager characterLocomotionManager;


        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);

            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
        }

        protected virtual void Start()
        {
            IgnoreMyOwnColliders();
        }

        protected virtual void Update()
        {
            //animator.SetBool("isGrounded", characterLocomotionManager.isGrounded);

        }

        protected virtual void LateUpdate()
        {

        }

        protected virtual void IgnoreMyOwnColliders()
        {
            Collider characterControllerColliders = GetComponent<Collider>();
            Collider[] damageableCharacterColliders = GetComponentsInChildren<Collider>();

            List<Collider> ignoreColliders = new List<Collider>();

            //ADD ALL OF OUR DAMAGEABLE CHARACTER COLLIDERS, TO THE LIST THAT WILL BE USED TO IGNORE COLLISIONS
            foreach (var collider in damageableCharacterColliders)
            {
                ignoreColliders.Add(collider);
            }

            //ADD ALL OF CHARACTER CONTROLLER COLLIDERS,AND IGNORE COLLISIONS WITH EACH OTHER
            ignoreColliders.Add(characterControllerColliders);

            //GOES THROUGH EVERY COLIDER ON THE LIST, AND IGNORES COLLISION WITH EACH OTHER
            foreach (var collider in ignoreColliders)
            {
                foreach (var otherCollider in ignoreColliders)
                {
                    Physics.IgnoreCollision(collider, otherCollider, true);
                }
            }
        }
    }

}