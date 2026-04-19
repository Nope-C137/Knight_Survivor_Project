using UnityEngine;

namespace TD
{
    /// <summary>
    /// Script cơ bản của tất cả các melee weapon behaviour [được sử dụng cho các loại vũ khí cận chiến]
    /// </summary>
    public class MeleeWeaponBehaviour : MonoBehaviour
    {
        public float destroyAfterSeconds = 5f; // Thời gian sau đó projectile sẽ tự hủy

        protected virtual void Start()
        {
            //Destroy(gameObject, destroyAfterSeconds); // Hủy projectile sau một khoảng thời gian
        }
    }

}