using UnityEngine;

namespace TD
{
    [CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "ScriptableObjects/Enemy")]
    public class EnemyScriptableObject : ScriptableObject
    {
        // Basic stats for all enemies
        //movement settings
        [Header("Enemy AI Settings")]
        [SerializeField] private float moveSpeed = 5f;
        public float MoveSpeed { get => moveSpeed; private set => moveSpeed = value; }
        [SerializeField] private float rotationSpeed = 8f;
        public float RotationSpeed { get => rotationSpeed; private set => rotationSpeed = value; }
        [SerializeField] private LayerMask groundLayer;
        public LayerMask GroundLayer { get => groundLayer; private set => groundLayer = value; }
        [SerializeField] private float gravityForce = 25f;
        public float GravityForce { get => gravityForce; private set => gravityForce = value; }

        //climbing settings
        [Header("Climbing")]
        [SerializeField] private float climbSpeed = 3f;
        public float ClimbSpeed { get => climbSpeed; private set => climbSpeed = value; }
        [SerializeField] private LayerMask wallLayer;
        public LayerMask WallLayer { get => wallLayer; private set => wallLayer = value; }
        [SerializeField] private float climbHopForce = 6f;
        public float ClimbHopForce { get => climbHopForce; private set => climbHopForce = value; }

        //attack settings
        [Header("Attack Settings")]
        [SerializeField] private float attackRange = 2f;
        public float AttackRange { get => attackRange; private set => attackRange = value; }
        [SerializeField] private float timeBetweenAttacks = 1.5f;
        public float TimeBetweenAttacks { get => timeBetweenAttacks; private set => timeBetweenAttacks = value; }
        [SerializeField] private float damage = 10f;
        public float Damage { get => damage; private set => damage = value; }

        //health settings
        [Header("Health Settings")]
        [SerializeField] private float maxHealth = 100;
        public float MaxHealth { get => maxHealth; private set => maxHealth = value; }
    }
}