using UnityEngine;

namespace TD
{
    public class Enemy_Stats : MonoBehaviour
    {
        public EnemyScriptableObject enemyData;

        //Current stats
        [HideInInspector] public float currentHealth;
        [HideInInspector] public float currentMoveSpeed;
        [HideInInspector] public float currentDamage;
        [HideInInspector] public float currentRotationSpeed;
        [HideInInspector] public float currentGravityForce;
        [HideInInspector] public float currentClimbSpeed;
        [HideInInspector] public float currentClimbHopForce;
        [HideInInspector] public float currentAttackRange;
        [HideInInspector] public float currentTimeBetweenAttacks;
        [HideInInspector] public LayerMask currentGroundLayer;
        [HideInInspector] public LayerMask currentWallLayer;

        private bool isDead = false;

        private void Awake()
        {
            currentHealth = enemyData.MaxHealth;
            currentMoveSpeed = enemyData.MoveSpeed;
            currentDamage = enemyData.Damage;
            currentRotationSpeed = enemyData.RotationSpeed;
            currentGravityForce = enemyData.GravityForce;
            currentClimbSpeed = enemyData.ClimbSpeed;
            currentClimbHopForce = enemyData.ClimbHopForce;
            currentAttackRange = enemyData.AttackRange;
            currentTimeBetweenAttacks = enemyData.TimeBetweenAttacks;
            currentGroundLayer = enemyData.GroundLayer;
            currentWallLayer = enemyData.WallLayer;
        }

        public void TakeDamage(float damage)
        {
            if (isDead) return;

            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                Death();
            }
        }

        private void Death()
        {
            isDead = true;

            // Delay nhỏ trước khi destroy (cho animation die)
            Invoke(nameof(DestroyEnemy), 0.5f);
        }

        private void DestroyEnemy()
        {
            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            // Gọi TakeDamage khi va chạm với player (giả sử player có tag "Player")
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
                player.TakeDamage(currentDamage); //Đảm bảo player có phương thức TakeDamage
            }
        }

        // Public methods
        //public void ResetHealth()
        //{
        //    currentHealth = enemyData.MaxHealth;
        //    isDead = false;
        //}

        //public float GetHealthPercentage() => Mathf.Clamp01((float)currentHealth / enemyData.MaxHealth);
    }
}