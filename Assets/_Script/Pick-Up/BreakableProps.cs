using UnityEngine;

namespace TD
{
    public class BreakableProps : MonoBehaviour
    {
        public float health;

        public void TakeDamage(float damage)
        {
            health -= damage;
            if (health <= 0)
            {
                Death();
            }
        }

        public void Death()
        {
            Destroy(gameObject);
        }
    }
}