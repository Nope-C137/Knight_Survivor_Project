using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int scoreOnDeath = 100;

    private int currentHealth;
    private bool isDead = false;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bullet") && !isDead)
        {
            TakeDamage(25);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        // Cộng điểm và kill qua ScoreManager
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddKill(scoreOnDeath);
        }
        else
        {
            Debug.LogWarning("ScoreManager.Instance is null! Score will not be added.");
        }

        // Delay nhỏ trước khi destroy (cho animation die)
        Invoke(nameof(DestroyEnemy), 0.5f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    // Public methods
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isDead = false;
    }

    public float GetHealthPercentage() => Mathf.Clamp01((float)currentHealth / maxHealth);
}