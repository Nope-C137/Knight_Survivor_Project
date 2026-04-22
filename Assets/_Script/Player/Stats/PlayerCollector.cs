using UnityEngine;

namespace TD
{
    public class PlayerCollector : MonoBehaviour
    {
        private void OnTriggerEnter(Collider collider)
        {
            //Kiểm tra xem đối tượng va chạm có thành phần ICollectible không
            if (collider.gameObject.TryGetComponent(out ICollectible collectible))
            {
                // Nếu có, gọi phương thức Collect() để thu thập đối tượng
                collectible.Collect();
            }
        }
    }
}