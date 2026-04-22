using UnityEngine;

namespace TD
{
    public class PlayerCollector : MonoBehaviour
    {
        private PlayerStats playerStats;
        private SphereCollider playerCollector;
        public float pullSpeed = 5f; // Tốc độ kéo đối tượng về phía player

        private void Start()
        {
            playerStats = FindFirstObjectByType<PlayerStats>();
            playerCollector = GetComponent<SphereCollider>();
        }

        private void Update()
        {
            // Cập nhật bán kính thu thập dựa trên thuộc tính currentMagnet của playerStats
            playerCollector.radius = playerStats.currentMagnet;
        }

        private void OnTriggerEnter(Collider collider)
        {
            //Kiểm tra xem đối tượng va chạm có thành phần ICollectible không
            if (collider.gameObject.TryGetComponent(out ICollectible collectible))
            {
                //Pulling Animation

                Rigidbody rb = collider.gameObject.GetComponent<Rigidbody>();
                Vector3 forceDirection = (transform.position - collider.transform.position);
                forceDirection.y = 0;
                rb.AddForce(forceDirection.normalized * pullSpeed, ForceMode.Force);

                // Nếu có, gọi phương thức Collect() để thu thập đối tượng
                collectible.Collect();
            }
        }
    }
}