using UnityEngine;
using System.Drawing;

namespace TD
{
    public class KnifeController : WeaponController
    {
        public UnityEngine.Color color = UnityEngine.Color.red;
        public float radius = 5f;

        protected override void Start()
        {
            base.Start();
        }

        protected override void Attack()
        {
            base.Attack();

            if (weaponData.Prefab == null) return;

            // Tính toán hướng và vị trí như cũ tinh toán vị trí di chuyển dựa theo movement
            //Vector3 throwDirection = playerController.transform.forward;
            //Vector3 moveVel = playerController.GetComponent<CharacterController>().velocity;
            //if (moveVel.magnitude > 0.1f) throwDirection = new Vector3(moveVel.x, 0, moveVel.z).normalized;
            //Vector3 spawnPos = playerController.transform.position + Vector3.up * 1.5f + throwDirection * 1.2f;

            // RADAR TÌM MỤC TIÊU
            Vector3 throwDirection = GetRadarDirection();

            // TÍNH TOÁN VỊ TRÍ SPAWN
            // Giữ nguyên logic spawn nhưng dùng throwDirection mới từ Radar
            Vector3 spawnPos = playerController.transform.position + Vector3.up * 1.5f + throwDirection * 1.2f;

            // Giả sử bạn đặt Tag trong PoolManager là "Knife"
            GameObject spawnedKnife = ObjectPoolManager.Instance.GetPooledObject("Knife");

            if (spawnedKnife != null)
            {
                spawnedKnife.transform.position = spawnPos;
                spawnedKnife.transform.rotation = Quaternion.LookRotation(throwDirection);
                spawnedKnife.SetActive(true); // Kích hoạt lại để nó hiện ra

                spawnedKnife.GetComponent<KnifeBehaviour>().Initialize(throwDirection);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = color;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
        private Vector3 GetRadarDirection()
        {
            // Mặc định là hướng phía trước của Player
            Vector3 finalDir = playerController.transform.forward;

            // Quét tất cả Collider trong vòng tròn
            Collider[] targets = Physics.OverlapSphere(transform.position, radius);

            float closestDistance = Mathf.Infinity;
            Transform closestTarget = null;

            foreach (var col in targets)
            {
                // Kiểm tra Tag
                if (col.CompareTag("Props"))
                {
                    Debug.Log("Nhận diện enemy");
                    float distance = Vector3.Distance(transform.position, col.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestTarget = col.transform;
                    }
                }
                else if (col.CompareTag("Enemy"))
                {
                    Debug.Log("Nhanaskdnkolas");
                    float distance = Vector3.Distance(transform.position, col.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestTarget = col.transform;
                    }
                }
            }

            // Nếu tìm thấy mục tiêu gần nhất, tính toán hướng về phía đó
            if (closestTarget != null)
            {
                finalDir = (closestTarget.position - (transform.position + Vector3.up * 1.5f)).normalized;
                Debug.DrawLine(transform.position + Vector3.up * 1.5f, closestTarget.position, UnityEngine.Color.yellow, 0.5f);

            }
            else
            {
                // Nếu không có mục tiêu, dùng logic di chuyển cũ của bạn
                Vector3 moveVel = playerController.GetComponent<CharacterController>().velocity;
                if (moveVel.magnitude > 0.1f) finalDir = new Vector3(moveVel.x, 0, moveVel.z).normalized;
            }

            return finalDir;
        }
    }
}