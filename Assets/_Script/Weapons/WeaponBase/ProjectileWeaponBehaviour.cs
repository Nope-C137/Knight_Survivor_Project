using UnityEngine;

namespace TD
{
    /// <summary>
    /// Script cơ bản của tất cả các projectile behaviour [được sử dụng cho các loại vũ khí bắn ra projectile]
    /// </summary>
    public class ProjectileWeaponBehaviour : MonoBehaviour
    {
        public WeaponScriptableObject weaponData; // Tham chiếu đến WeaponScriptableObject để lấy thông tin về projectile
        protected Vector3 direction; // Hướng di chuyển của projectile
        public float destroyAfterSeconds = 5f; // Thời gian sau đó projectile sẽ tự hủy

        //Current Stats
        protected float currentDamage;
        protected float currentSpeed;
        protected float currentCooldownDuration;
        protected int currentPierce;

        private void Awake()
        {
            currentDamage = weaponData.Damage;
            currentSpeed = weaponData.Speed;
            currentCooldownDuration = weaponData.CooldownDuration;
            currentPierce = weaponData.Pierce;
        }

        protected virtual void Start()
        {
            //Destroy(gameObject, destroyAfterSeconds); // Hủy projectile sau một khoảng thời gian
        }

        public void DirectionChecker(Vector3 dir)
        {
            direction = dir.normalized;
        }

        protected virtual void OnTriggerEnter(Collider collider)
        {
            // Kiểm tra nếu va chạm với một đối tượng có tag "Enemy"
            if (collider.CompareTag("Enemy"))
            {
                EnemyHealth enemy = collider.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage(currentDamage); // Đảm bảo rằng TakeDamge được gọi đúng cách với currentDamage không phải weaponData.Damage
                    ReducePierce(); // Giảm pierce sau khi va chạm với enemy
                }
            }
            else if(collider.CompareTag("Props"))
            {
                if (collider.gameObject.TryGetComponent(out BreakableProps breakable))
                {
                    breakable.TakeDamage(currentDamage); // Đảm bảo rằng TakeDamge được gọi đúng cách với currentDamage không phải weaponData.Damage
                    ReducePierce(); // Giảm pierce sau khi va chạm với props
                }
            }
        }

        private void ReducePierce() // Destroy projectile sau khi pierce hết = 0
        {
            currentPierce--;
            if (currentPierce <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}