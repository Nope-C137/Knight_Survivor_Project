using UnityEngine;

namespace TD
{
    public class EnemyHealth : MonoBehaviour
    {
        public EnemyScriptableObject enemyData;
        private float currentHealth;
        private float currentMoveSpeed;
        private float currentDamage;

        private bool isDead = false;

        private void Awake()
        {
            currentHealth = enemyData.MaxHealth;
            currentMoveSpeed = enemyData.MoveSpeed;
            currentDamage = enemyData.Damage;
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

        // Public methods
        //public void ResetHealth()
        //{
        //    currentHealth = enemyData.MaxHealth;
        //    isDead = false;
        //}

        //public float GetHealthPercentage() => Mathf.Clamp01((float)currentHealth / enemyData.MaxHealth);
    }
}