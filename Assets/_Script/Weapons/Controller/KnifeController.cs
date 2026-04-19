using UnityEngine;

namespace TD
{
    public class KnifeController : WeaponController
    {        
        protected override void Start()
        {
            base.Start();
        }

        protected override void Attack()
        {
            base.Attack();

            if (weaponData.Prefab == null) return;

            // Tính toán hướng và vị trí như cũ
            Vector3 throwDirection = playerController.transform.forward;
            Vector3 moveVel = playerController.GetComponent<CharacterController>().velocity;
            if (moveVel.magnitude > 0.1f) throwDirection = new Vector3(moveVel.x, 0, moveVel.z).normalized;
            Vector3 spawnPos = playerController.transform.position + Vector3.up * 1.5f + throwDirection * 1.2f;

            // Giả sử bạn đặt Tag trong PoolManager là "Knife"
            GameObject spawnedKnife = ObjectPoolManager.Instance.GetPooledObject("Knife");

            if (spawnedKnife != null)
            {
                spawnedKnife.transform.position = spawnPos;
                spawnedKnife.transform.rotation = Quaternion.LookRotation(throwDirection);
                spawnedKnife.SetActive(true); // Kích hoạt lại để nó hiện ra

                spawnedKnife.GetComponent<KnifeBehaviour>().Initialize(throwDirection, weaponData.Speed);
            }
        }
    }
}