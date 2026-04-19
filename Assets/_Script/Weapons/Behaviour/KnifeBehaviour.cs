using UnityEngine;

namespace TD
{
    public class KnifeBehaviour : ProjectileWeaponBehaviour
    {
        private float timer;
        private Rigidbody rb;

        // Khởi tạo từ KnifeController
        public void Initialize(Vector3 dir, float spd)
        {
            rb = GetComponent<Rigidbody>();
            timer = destroyAfterSeconds; // Reset lại thời gian sống mỗi khi lấy ra từ pool

            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                rb.linearVelocity = dir.normalized * spd;
            }
        }

        protected override void Start()
        {
            base.Start();
           
        }

        private void Update()
        {
            // Tự động ẩn sau X giây thay vì dùng Destroy
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                gameObject.SetActive(false);
            }
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