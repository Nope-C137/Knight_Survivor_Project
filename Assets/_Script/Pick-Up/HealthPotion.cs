using UnityEngine;

namespace TD
{
    public class HealthPotion : MonoBehaviour, ICollectible
    {
        public int healhRestoreAmount = 50; // Số lượng máu hồi phục khi thu thập bình máu
        public void Collect()
        {
            PlayerStats player = FindFirstObjectByType<PlayerStats>();
            player.ResstoreHealth(healhRestoreAmount);
            Destroy(gameObject); // Hủy đối tượng bình máu sau khi thu thập
        }
    }

}