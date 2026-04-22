using UnityEngine;

namespace TD
{
    public class Pick_Up_Manager : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) // Kiểm tra nếu va chạm với player để tránh va chạm với các đối tượng khác
            {
                Destroy(gameObject);
            }
        }
    }
}