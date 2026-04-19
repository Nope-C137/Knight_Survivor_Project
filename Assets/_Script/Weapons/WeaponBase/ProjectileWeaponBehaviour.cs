using UnityEngine;

namespace TD
{
    /// <summary>
    /// Script cơ bản của tất cả các projectile behaviour [được sử dụng cho các loại vũ khí bắn ra projectile]
    /// </summary>
    public class ProjectileWeaponBehaviour : MonoBehaviour
    {
        protected Vector3 direction; // Hướng di chuyển của projectile
        public float destroyAfterSeconds = 5f; // Thời gian sau đó projectile sẽ tự hủy

        protected virtual void Start()
        {
            //Destroy(gameObject, destroyAfterSeconds); // Hủy projectile sau một khoảng thời gian
        }

        public void DirectionChecker(Vector3 dir)
        {
            direction = dir.normalized;
        }
    }

}