using UnityEngine;

namespace TD
{
    public class GarlicBehaviour : MeleeWeaponBehaviour
    {
        private float timer;
        public float rotationSpeed = 50f; // Tốc độ xoay của khiên

        public void Initialize(Vector3 dir, float spd)
        {
            timer = destroyAfterSeconds; // Reset lại thời gian sống mỗi khi lấy ra từ pool

        }

        protected override void Start()
        {
            base.Start();
        }

        private void Update()
        {
            // Tự động xoay cả object Body (chứa 4 cái sprite tỏi)
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            // Tự động ẩn sau X giây thay vì dùng Destroy
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                DeactivateGarlic();
            }
        }

        private void DeactivateGarlic()
        {
            // 1. Trả Parent về null trước
            transform.SetParent(null);

            // 2. Sau đó mới tắt Object
            gameObject.SetActive(false);
        }

        private void OnCollisionEnter(Collision collision)
        {
            // Tránh va chạm với chính người chơi
            if (collision.gameObject.GetComponent<PlayerController>() == null)
            {
                gameObject.SetActive(false); // Thay vì Destroy
            }
        }
    }
}