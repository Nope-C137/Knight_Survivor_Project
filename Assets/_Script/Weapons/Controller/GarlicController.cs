using UnityEngine;

namespace TD
{
    public class GarlicController : WeaponController
    {
        private GameObject activeGarlicShield;

        protected override void Start()
        {
            base.Start();
        }

        protected override void Attack()
        {
            base.Attack();

            if (weaponData.Prefab == null) return;

            // Nếu chưa có khiên hoặc khiên cũ đã bị ẩn, thì lấy cái mới từ Pool
            if (activeGarlicShield == null || !activeGarlicShield.activeInHierarchy)
            {
                activeGarlicShield = ObjectPoolManager.Instance.GetPooledObject("Garlic");
            }

            if (activeGarlicShield != null)
            {
                // ĐẶT PLAYER VÀO GIỮA:
                // 1. Đặt vị trí khiên trùng với vị trí Player (có bù chiều cao Y)
                activeGarlicShield.transform.position = playerController.transform.position + Vector3.up * 1f;

                // 2. Gắn khiên làm con của Player để nó luôn đi theo khi di chuyển
                activeGarlicShield.transform.SetParent(playerController.transform);

                activeGarlicShield.SetActive(true);

                // Khởi tạo logic (như thời gian tồn tại)
                if (activeGarlicShield.TryGetComponent<GarlicBehaviour>(out var behaviour))
                {
                    behaviour.Initialize(Vector3.zero, weaponData.Speed);
                }
            }
        }
    }

}