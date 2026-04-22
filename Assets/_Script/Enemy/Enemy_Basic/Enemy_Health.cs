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