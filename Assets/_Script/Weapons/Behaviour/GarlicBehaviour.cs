using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

namespace TD
{
    public class GarlicBehaviour : MeleeWeaponBehaviour
    {
        List<GameObject> markedEnemies;
        private float timer;
        private Rigidbody rb;
        //public float rotationSpeed = 50f; // Tốc độ xoay của khiên rotationSpeed = weaponData.Speed

        public void Initialize(Vector3 dir, float spd)
        {
            rb = GetComponent<Rigidbody>();
            timer = destroyAfterSeconds; // Reset lại thời gian sống mỗi khi lấy ra từ pool
            markedEnemies = new List<GameObject>();
        }

        protected override void Start()
        {
            base.Start();
        }

        private void Update()
        {
            // Tự động xoay cả object Body (chứa 4 cái sprite tỏi)
            transform.Rotate(Vector3.up, weaponData.Speed * Time.deltaTime);

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

        protected override void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag("Enemy") && !markedEnemies.Contains(collider.gameObject))
            {
                Enemy_Stats enemy = collider.GetComponent<Enemy_Stats>();
                if (enemy != null)
                {
                    enemy.TakeDamage(currentDamage);
                    markedEnemies.Add(collider.gameObject); // Đánh dấu để không bị trừ máu nhiều lần khi va chạm với cùng một enemy
                }
            }
            else if(collider.CompareTag("Props"))
            {
                if (collider.gameObject.TryGetComponent(out BreakableProps breakable) && !markedEnemies.Contains(collider.gameObject))
                {
                    breakable.TakeDamage(currentDamage);
                    markedEnemies.Add(collider.gameObject); // Đánh dấu để không bị trừ máu nhiều lần khi va chạm với cùng một props
                }
            }
        }
    }
}