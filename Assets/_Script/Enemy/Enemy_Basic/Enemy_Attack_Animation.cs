using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TD
{
    public class EnemyAttackAnimation : MonoBehaviour
    {
        public Animator animator;

        private void Start()
        {
            animator = transform.Find("Weapon").GetComponentInChildren<Animator>();
        }

        public void PlayAttackAnimation()
        {
            animator.SetBool("isAttacking", true);
        }

        public void StopAttackAnimation()
        {
            animator.SetBool("isAttacking", false);
        }
    }
}