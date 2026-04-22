using Unity.VisualScripting;
using UnityEngine;

namespace TD
{
    public class HealthPotion : MonoBehaviour, ICollectible
    {
        public float rotateSpeed;
        public int healhRestoreAmount = 50; // Số lượng máu hồi phục khi thu thập bình máu
        public void Collect()
        {
            PlayerStats player = FindFirstObjectByType<PlayerStats>();
            player.ResstoreHealth(healhRestoreAmount);
            Destroy(gameObject); // Hủy đối tượng bình máu sau khi thu thập
        }
        

        private void Update()
        {
            Handle_Rotation();
        }
        private void Handle_Rotation()
        {
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
        }
    }

}