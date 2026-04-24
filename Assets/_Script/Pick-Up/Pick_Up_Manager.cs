using UnityEngine;

namespace TD
{
    public class Pick_Up_Manager : MonoBehaviour
    {
        public float rotateSpeed;

        protected virtual void Update()
        {
            Handle_Rotation();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) // Kiểm tra nếu va chạm với player để tránh va chạm với các đối tượng khác
            {
                Destroy(gameObject);
            }
        }
        protected virtual void Handle_Rotation()
        {
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
        }
    }
}