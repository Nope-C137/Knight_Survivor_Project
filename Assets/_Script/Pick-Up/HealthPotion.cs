using UnityEngine;

namespace TD
{
    public class HealthPotion : Pick_Up_Manager, ICollectible
    {
        public int healhRestoreAmount = 50; // Số lượng máu hồi phục khi thu thập bình máu
        public void Collect()
        {
            PlayerStats player = FindFirstObjectByType<PlayerStats>();
            player.ResstoreHealth(healhRestoreAmount);
        }
    }
}