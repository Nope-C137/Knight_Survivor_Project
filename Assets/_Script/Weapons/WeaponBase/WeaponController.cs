using UnityEngine;

namespace TD
{
    /// <summary>
    /// Script cơ bản cho mọi weapon controllers 
    /// </summary>
    public class WeaponController : MonoBehaviour
    {
        public WeaponScriptableObject weaponData;
        public float currentCooldown;

        protected PlayerController playerController;

        protected virtual void Start()
        {
            playerController = FindAnyObjectByType<PlayerController>();
            currentCooldown = weaponData.CooldownDuration; // Khởi tạo cooldown để có thể bắn ngay khi bắt đầu
        }

        protected virtual void Update()
        {
            currentCooldown -= Time.deltaTime; // Giảm cooldown theo thời gian
            if(currentCooldown <= 0f) // Nếu cooldown đã hết bằng 0, có thể bắn Attack
            {
                Attack();
            }
        }

        protected virtual void Attack()
        {
            currentCooldown = weaponData.CooldownDuration; // Reset cooldown sau khi bắn
        }
    }
}